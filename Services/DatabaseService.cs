using Microsoft.Data.Sqlite;
using CdiskClean.Models;

namespace CdiskClean.Services;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string dbPath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"PRAGMA journal_mode=WAL;

            CREATE TABLE IF NOT EXISTS WatchDirectories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Path TEXT NOT NULL UNIQUE,
                IncludeSubdirs INTEGER NOT NULL DEFAULT 1,
                Status TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ChangeRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Timestamp TEXT NOT NULL,
                ChangeType TEXT NOT NULL,
                FullPath TEXT NOT NULL,
                FileName TEXT NOT NULL,
                Directory TEXT NOT NULL,
                SizeBytes INTEGER,
                SourceProcess TEXT,
                CreatedAt TEXT NOT NULL
            );";

        cmd.ExecuteNonQuery();
    }

    public List<WatchingDirectory> GetWatchDirectories()
    {
        var list = new List<WatchingDirectory>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Path, IncludeSubdirs, Status FROM WatchDirectories WHERE Status != 'DELETED';";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var dir = new WatchingDirectory(
                reader.GetString(0),
                reader.GetInt32(1) != 0)
            {
                Status = Enum.TryParse<DirectoryStatusEnum>(reader.GetString(2), out var s)
                    ? s : DirectoryStatusEnum.USING
            };
            list.Add(dir);
        }

        return list;
    }

    public void SaveWatchDirectory(WatchingDirectory dir)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO WatchDirectories (Path, IncludeSubdirs, Status, CreatedAt, UpdatedAt)
                            VALUES (@Path, @Subdirs, @Status, @Now, @Now)
                            ON CONFLICT(Path) DO UPDATE SET
                                IncludeSubdirs = @Subdirs,
                                Status = @Status,
                                UpdatedAt = @Now;";

        cmd.Parameters.AddWithValue("@Path", dir.Path);
        cmd.Parameters.AddWithValue("@Subdirs", dir.IncludeSubdirs ? 1 : 0);
        cmd.Parameters.AddWithValue("@Status", dir.Status.ToString());
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        cmd.ExecuteNonQuery();
    }

    public void DeleteWatchDirectory(string path)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM WatchDirectories WHERE Path = @Path;";
        cmd.Parameters.AddWithValue("@Path", path);
        cmd.ExecuteNonQuery();
    }

    public void SaveChangeRecord(FileChangeRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO ChangeRecords (Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess, CreatedAt)
                            VALUES (@Ts, @Type, @FullPath, @FileName, @Dir, @Size, @Source, @Now);";

        cmd.Parameters.AddWithValue("@Ts", record.Timestamp.ToString("O"));
        cmd.Parameters.AddWithValue("@Type", record.ChangeType.ToString());
        cmd.Parameters.AddWithValue("@FullPath", record.FullPath);
        cmd.Parameters.AddWithValue("@FileName", record.FileName);
        cmd.Parameters.AddWithValue("@Dir", record.Directory);
        cmd.Parameters.AddWithValue("@Size", record.SizeBytes.HasValue ? (object)record.SizeBytes.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Source", DBNull.Value); // 后续实现来源识别
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        cmd.ExecuteNonQuery();
    }

    public List<FileChangeRecord> GetChangeRecords(int limit = 1000)
    {
        var list = new List<FileChangeRecord>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes FROM ChangeRecords ORDER BY Id DESC LIMIT @Limit;";
        cmd.Parameters.AddWithValue("@Limit", limit);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new FileChangeRecord
            {
                Timestamp = DateTime.Parse(reader.GetString(0)),
                ChangeType = Enum.Parse<ChangeType>(reader.GetString(1)),
                FullPath = reader.GetString(2),
                FileName = reader.GetString(3),
                Directory = reader.GetString(4),
                SizeBytes = reader.IsDBNull(5) ? null : reader.GetInt64(5)
            });
        }

        return list;
    }

    public void ClearChangeRecords()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM ChangeRecords;";
        cmd.ExecuteNonQuery();
    }
}
