using Microsoft.Data.Sqlite;
using CdiskClean.Models;
using CdiskClean.Models.cleanUp;

namespace CdiskClean.Services.database;

public sealed class HistoryStore
{
    private readonly SqliteDatabaseService _database;

    internal HistoryStore(SqliteDatabaseService database) => _database = database;

    public void SaveChangeRecord(FileChangeRecord record)
    {
        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO ChangeRecords (Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess, CreatedAt)
                            VALUES (@Ts, @Type, @FullPath, @FileName, @Dir, @Size, @Source, @Now);";

        cmd.Parameters.AddWithValue("@Ts", record.Timestamp.ToString("O"));
        cmd.Parameters.AddWithValue("@Type", record.ChangeType.ToString());
        cmd.Parameters.AddWithValue("@FullPath", record.FullPath);
        cmd.Parameters.AddWithValue("@FileName", record.FileName);
        cmd.Parameters.AddWithValue("@Dir", record.Directory);
        cmd.Parameters.AddWithValue("@Size", record.SizeBytes.HasValue ? (object)record.SizeBytes.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Source", record.SourceProcess ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        cmd.ExecuteNonQuery();
    }

    public List<FileChangeRecord> GetChangeRecords(int limit = 1000)
    {
        var list = new List<FileChangeRecord>();

        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess FROM ChangeRecords ORDER BY Id DESC LIMIT @Limit;";
        cmd.Parameters.AddWithValue("@Limit", limit);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(ReadChangeRecord(reader));
        }

        return list;
    }

    public List<FileChangeRecord> GetChangeRecordsUnderPath(string path, int limit = 100)
    {
        var list = new List<FileChangeRecord>();
        using var connection = _database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess
                                FROM ChangeRecords
                                WHERE FullPath = @Path OR FullPath LIKE @Prefix ESCAPE '\'
                                ORDER BY Id DESC LIMIT @Limit;";
        command.Parameters.AddWithValue("@Path", path);
        command.Parameters.AddWithValue("@Prefix", EscapeLikePattern(path.TrimEnd('\\')) + "\\\\%");
        command.Parameters.AddWithValue("@Limit", Math.Clamp(limit, 1, 5000));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(ReadChangeRecord(reader));
        }
        return list;
    }

    private static FileChangeRecord ReadChangeRecord(SqliteDataReader reader) => new()
    {
        Timestamp = DateTime.Parse(reader.GetString(0)),
        ChangeType = Enum.Parse<ChangeType>(reader.GetString(1)),
        FullPath = reader.GetString(2),
        FileName = reader.GetString(3),
        Directory = reader.GetString(4),
        SizeBytes = reader.IsDBNull(5) ? null : reader.GetInt64(5),
        SourceProcess = reader.IsDBNull(6) ? null : reader.GetString(6)
    };

    private static string EscapeLikePattern(string value) =>
        value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");

    public void SaveProcessNotification(ProcessNotificationRecord record)
    {
        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO ProcessNotifications (ProcessName, OperationCount, DurationSeconds, TriggerTime, CreatedAt)
                            VALUES (@Name, @Count, @Duration, @TriggerTime, @Now);";

        cmd.Parameters.AddWithValue("@Name", record.ProcessName);
        cmd.Parameters.AddWithValue("@Count", record.OperationCount);
        cmd.Parameters.AddWithValue("@Duration", record.DurationSeconds);
        cmd.Parameters.AddWithValue("@TriggerTime", record.TriggerTime.ToString("O"));
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        cmd.ExecuteNonQuery();
    }

    public List<ProcessNotificationRecord> GetProcessNotifications(int limit = 200)
    {
        var list = new List<ProcessNotificationRecord>();

        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT ProcessName, OperationCount, DurationSeconds, TriggerTime FROM ProcessNotifications ORDER BY Id DESC LIMIT @Limit;";
        cmd.Parameters.AddWithValue("@Limit", limit);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new ProcessNotificationRecord
            {
                ProcessName = reader.GetString(0),
                OperationCount = reader.GetInt32(1),
                DurationSeconds = reader.GetInt32(2),
                TriggerTime = DateTime.Parse(reader.GetString(3))
            });
        }

        return list;
    }

    public void SaveCleanupRecord(CleanupRecord record)
    {
        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO CleanupRecords (CleanupTime, FullPath, FileName, SizeBytes, Method, Category, Success, Message, CreatedAt)
                            VALUES (@Time, @FullPath, @FileName, @Size, @Method, @Category, @Success, @Message, @Now);";

        cmd.Parameters.AddWithValue("@Time", record.CleanupTime.ToString("O"));
        cmd.Parameters.AddWithValue("@FullPath", record.FullPath);
        cmd.Parameters.AddWithValue("@FileName", record.FileName);
        cmd.Parameters.AddWithValue("@Size", record.SizeBytes.HasValue ? (object)record.SizeBytes.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@Method", record.Method);
        cmd.Parameters.AddWithValue("@Category", record.Category);
        cmd.Parameters.AddWithValue("@Success", record.Success ? 1 : 0);
        cmd.Parameters.AddWithValue("@Message", record.Message ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));

        cmd.ExecuteNonQuery();
    }

    public List<CleanupRecord> GetCleanupRecords(int limit = 200)
    {
        var list = new List<CleanupRecord>();

        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = @"SELECT Id, CleanupTime, FullPath, FileName, SizeBytes, Method, Category, Success, Message
                            FROM CleanupRecords ORDER BY Id DESC LIMIT @Limit;";
        cmd.Parameters.AddWithValue("@Limit", limit);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new CleanupRecord
            {
                Id = reader.GetInt64(0),
                CleanupTime = DateTime.Parse(reader.GetString(1)),
                FullPath = reader.GetString(2),
                FileName = reader.GetString(3),
                SizeBytes = reader.IsDBNull(4) ? null : reader.GetInt64(4),
                Method = reader.GetString(5),
                Category = reader.IsDBNull(6) ? CleanupCategory.Other.GetDisplayName() : reader.GetString(6),
                Success = reader.GetInt32(7) != 0,
                Message = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }

        return list;
    }
}
