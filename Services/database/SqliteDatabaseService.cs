using Microsoft.Data.Sqlite;
using CdiskClean.Models;
using System.Data.Common;
using CdiskClean.Models.cleanUp;
using CdiskClean.Models.rules;

namespace CdiskClean.Services.database;

public class SqliteDatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public SqliteDatabaseService(string dbPath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            DefaultTimeout = 5,
            Pooling = true
        }.ToString();
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        // Id , CreatedAt 可以 使用 AUTOINCREMENT 和 DEFAULT CURRENT_TIMESTAMP 来自动生成和设置时间戳
        cmd.CommandText = (@"PRAGMA journal_mode=WAL; " +

            WatchingDirectory.GetCreateSQL() +

            FileChangeRecord.GetCreateSQL() +

            ProcessNotificationRecord.GetCreateSQL() +

            IgnoreProcessRecord.GetCreateSQL() +

            WatchingExeInfo.GetCreateSQL() +

            CleanupRecord.GetCreateSQL());

        cmd.ExecuteNonQuery();
        EnsureWatchingApplicationColumns(connection);
        TrimHistoryTables(connection);
    }

    private static void EnsureWatchingApplicationColumns(SqliteConnection connection)
    {
        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using (var query = connection.CreateCommand())
        {
            query.CommandText = "PRAGMA table_info(WatchingExeInfo);";
            using var reader = query.ExecuteReader();
            while (reader.Read()) columns.Add(reader.GetString(1));
        }

        if (!columns.Contains("UpdatedAt"))
            AddWatchingApplicationColumn(connection, "UpdatedAt TEXT NULL");
        if (!columns.Contains("LastActivityAt"))
            AddWatchingApplicationColumn(connection, "LastActivityAt TEXT NULL");
    }

    private static void AddWatchingApplicationColumn(SqliteConnection connection, string definition)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"ALTER TABLE WatchingExeInfo ADD COLUMN {definition};";
        command.ExecuteNonQuery();
    }

    private static void TrimHistoryTables(SqliteConnection connection)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            DELETE FROM ChangeRecords
            WHERE Id < COALESCE((
                SELECT MIN(Id) FROM (
                    SELECT Id FROM ChangeRecords ORDER BY Id DESC LIMIT 50000
                ) AS RecentChanges
            ), 0);
            DELETE FROM ProcessNotifications
            WHERE Id < COALESCE((
                SELECT MIN(Id) FROM (
                    SELECT Id FROM ProcessNotifications ORDER BY Id DESC LIMIT 5000
                ) AS RecentNotifications
            ), 0);
            DELETE FROM CleanupRecords
            WHERE Id < COALESCE((
                SELECT MIN(Id) FROM (
                    SELECT Id FROM CleanupRecords ORDER BY Id DESC LIMIT 5000
                ) AS RecentCleanups
            ), 0);";
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
                Status = Enum.TryParse<RecordStatusEnum>(reader.GetString(2), out var s)
                    ? s : RecordStatusEnum.USING
            };
            list.Add(dir);
        }

        return list;
    }
    #region WatchingDirectory 表操作
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

    #endregion
    #region ChangeRecords 表操作
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
        cmd.Parameters.AddWithValue("@Source", record.SourceProcess ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        cmd.ExecuteNonQuery();
    }

    public List<FileChangeRecord> GetChangeRecords(int limit = 1000)
    {
        var list = new List<FileChangeRecord>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess FROM ChangeRecords ORDER BY Id DESC LIMIT @Limit;";
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
                SizeBytes = reader.IsDBNull(5) ? null : reader.GetInt64(5),
                SourceProcess = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return list;
    }

    #endregion
    #region ProcessNotifications 表的操作

    public void SaveProcessNotification(ProcessNotificationRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

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

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

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

    #endregion

    #region IgnoreProcessRecord 表操作
    public void SaveIgnoreProcessRecord(IgnoreProcessRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // 先更新，不存在再插入（兼容旧库无 UNIQUE 约束的表结构）
        using (var updateCmd = connection.CreateCommand())
        {
            updateCmd.CommandText = @"UPDATE IgnoreProcessRecord SET Status = @Status
                                      WHERE ProcessName = @Name;";
            updateCmd.Parameters.AddWithValue("@Name", record.ProcessName);
            updateCmd.Parameters.AddWithValue("@Status", record.Status.ToString());
            if (updateCmd.ExecuteNonQuery() > 0) return;
        }

        using var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"INSERT INTO IgnoreProcessRecord (ProcessName, Status, CreatedAt)
                                  VALUES (@Name, @Status, @Now);";
        insertCmd.Parameters.AddWithValue("@Name", record.ProcessName);
        insertCmd.Parameters.AddWithValue("@Status", record.Status.ToString());
        insertCmd.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        insertCmd.ExecuteNonQuery();
    }

    public List<IgnoreProcessRecord> GetIgnoreProcessRecords(int limit = 200)
    {
        var list = new List<IgnoreProcessRecord>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT ProcessName, Status FROM IgnoreProcessRecord ORDER BY Id DESC LIMIT @Limit;";
        cmd.Parameters.AddWithValue("@Limit", limit);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new IgnoreProcessRecord(reader.GetString(0))
            {
                Status = Enum.TryParse<RecordStatusEnum>(reader.GetString(1), out var s)
                    ? s : RecordStatusEnum.USING
            });
        }
        return list;
    }

    public void DeleteIgnoreProcessRecord(string processName)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM IgnoreProcessRecord WHERE ProcessName = @Name;";
        cmd.Parameters.AddWithValue("@Name", processName);
        cmd.ExecuteNonQuery();
    }
    #endregion

    #region CleanupRecord 表操作
    public void SaveCleanupRecord(CleanupRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO CleanupRecords (CleanupTime, FullPath, FileName, SizeBytes, Method, Success, Message, CreatedAt)
                            VALUES (@Time, @FullPath, @FileName, @Size, @Method, @Success, @Message, @Now);";

        var paramList = new List<(string, object)>
        {
        ("@Time", record.CleanupTime.ToString("O")),
        ("@FullPath", record.FullPath),
        ("@FileName", record.FileName),
        ("@Size", record.SizeBytes.HasValue ? (object)record.SizeBytes.Value : DBNull.Value),
        ("@Method", record.Method),
        ("@Success", record.Success ? 1 : 0),
        ("@Message", record.Message ?? (object)DBNull.Value),
        ("@Now", DateTime.Now.ToString("O"))
        };


        foreach (var item in paramList)
        {
            cmd.Parameters.AddWithValue(item.Item1,item.Item2 );
        }


        cmd.ExecuteNonQuery();
    }

    public List<CleanupRecord> GetCleanupRecords(int limit = 200)
    {
        var list = new List<CleanupRecord>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        //cmd.CommandText = "SELECT COUNT(*) FROM CleanupRecords";
        //int nums = Convert.ToInt32(cmd.ExecuteScalar());

        //MessageBox.Show(nums.ToString());
        cmd.CommandText = @"SELECT Id, CleanupTime, FullPath, FileName, SizeBytes, Method, Success, Message
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
                Success = reader.GetInt32(6) != 0,
                Message = reader.IsDBNull(7) ? null : reader.GetString(7)
            });
        }

        return list;
    }
    #endregion


    #region MonitoringExeInfo
    public List<WatchingExeInfo> GetWatchingApplications()
    {
        var list = new List<WatchingExeInfo>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ExeName, FullPath, SizeBytes, RunningState, MonitoringState, LastActivityAt FROM WatchingExeInfo ORDER BY Id DESC;";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            long? size = null;
            if (!reader.IsDBNull(2) && long.TryParse(reader.GetValue(2)?.ToString(), out var parsed)) size = parsed;
            var runningState = reader.IsDBNull(3) ? "未知" : reader.GetString(3);
            var statusText = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            list.Add(new WatchingExeInfo
            {
                ExeName = reader.GetString(0),
                FullPath = reader.GetString(1),
                SizeBytes = size,
                RunningState = runningState,
                Status = statusText.Equals("USING", StringComparison.OrdinalIgnoreCase) || statusText.Equals("运行中", StringComparison.OrdinalIgnoreCase)
                    ? RecordStatusEnum.USING : RecordStatusEnum.FORBIDDEN,
                LastActivityAt = reader.IsDBNull(5) || !DateTime.TryParse(reader.GetString(5), out var activity) ? null : activity
            });
        }
        return list;
    }

    public List<FileChangeRecord> GetChangeRecordsUnderPath(string path, int limit = 100)
    {
        var list = new List<FileChangeRecord>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT Timestamp, ChangeType, FullPath, FileName, Directory, SizeBytes, SourceProcess
                                FROM ChangeRecords
                                WHERE FullPath = @Path OR FullPath LIKE @Prefix ESCAPE '\'
                                ORDER BY Id DESC LIMIT @Limit;";
        command.Parameters.AddWithValue("@Path", path);
        command.Parameters.AddWithValue("@Prefix", EscapeLikePattern(path.TrimEnd('\\')) + "\\%");
        command.Parameters.AddWithValue("@Limit", Math.Clamp(limit, 1, 5000));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new FileChangeRecord
            {
                Timestamp = DateTime.Parse(reader.GetString(0)),
                ChangeType = Enum.Parse<ChangeType>(reader.GetString(1)),
                FullPath = reader.GetString(2),
                FileName = reader.GetString(3),
                Directory = reader.GetString(4),
                SizeBytes = reader.IsDBNull(5) ? null : reader.GetInt64(5),
                SourceProcess = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }
        return list;
    }

    private static string EscapeLikePattern(string value) =>
        value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");

    public void SaveWatchingApplication(WatchingExeInfo application)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO WatchingExeInfo (ExeName, FullPath, SizeBytes, RunningState, MonitoringState, CreatedAt, UpdatedAt, LastActivityAt)
            VALUES (@Name, @Path, @Size, @Running, @Status, @Now, @Now, @Activity)
            ON CONFLICT(FullPath) DO UPDATE SET ExeName = @Name, SizeBytes = @Size, MonitoringState = @Status, UpdatedAt = @Now, LastActivityAt = @Activity;";
        command.Parameters.AddWithValue("@Name", application.ExeName);
        command.Parameters.AddWithValue("@Path", application.FullPath);
        command.Parameters.AddWithValue("@Size", application.SizeBytes.HasValue ? application.SizeBytes.Value : 0L);
        command.Parameters.AddWithValue("@Running", application.UsesProcessIdentity && System.Diagnostics.Process.GetProcessesByName(application.ExeName).Length > 0 ? "运行中" : "未运行");
        command.Parameters.AddWithValue("@Status", application.Status.ToString());
        command.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        command.Parameters.AddWithValue("@Activity", application.LastActivityAt?.ToString("O") ?? (object)DBNull.Value);
        command.ExecuteNonQuery();
    }

    public void DeleteWatchingApplication(string fullPath)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM WatchingExeInfo WHERE FullPath = @Path;";
        command.Parameters.AddWithValue("@Path", fullPath);
        command.ExecuteNonQuery();
    }

    public void UpdateWatchingApplicationActivity(string exeName, DateTime activityTime)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE WatchingExeInfo SET LastActivityAt = @Activity, UpdatedAt = @Now WHERE ExeName = @Name;";
        command.Parameters.AddWithValue("@Name", WatchingExeInfo.NormalizeProcessName(exeName));
        command.Parameters.AddWithValue("@Activity", activityTime.ToString("O"));
        command.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        command.ExecuteNonQuery();
    }

    #endregion


}
