using Microsoft.Data.Sqlite;
using CdiskClean.Models;
using CdiskClean.Models.cleanUp;
using CdiskClean.Models.rules;

namespace CdiskClean.Services.database;

public sealed class MonitoringRuleStore
{
    private readonly SqliteDatabaseService _database;

    internal MonitoringRuleStore(SqliteDatabaseService database) => _database = database;

    public List<WatchingDirectory> GetWatchDirectories()
    {
        var list = new List<WatchingDirectory>();

        using var connection = _database.OpenConnection();

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

    public void SaveWatchDirectory(WatchingDirectory dir)
    {
        using var connection = _database.OpenConnection();

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
        using var connection = _database.OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM WatchDirectories WHERE Path = @Path;";
        cmd.Parameters.AddWithValue("@Path", path);
        cmd.ExecuteNonQuery();
    }

    public void SaveIgnoreProcessRecord(IgnoreProcessRecord record)
    {
        using var connection = _database.OpenConnection();

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
        using var connection = _database.OpenConnection();
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
        using var connection = _database.OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM IgnoreProcessRecord WHERE ProcessName = @Name;";
        cmd.Parameters.AddWithValue("@Name", processName);
        cmd.ExecuteNonQuery();
    }

    public List<WatchingExeInfo> GetWatchingApplications()
    {
        var list = new List<WatchingExeInfo>();
        using var connection = _database.OpenConnection();
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

    public void SaveWatchingApplication(WatchingExeInfo application)
    {
        using var connection = _database.OpenConnection();
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
        using var connection = _database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM WatchingExeInfo WHERE FullPath = @Path;";
        command.Parameters.AddWithValue("@Path", fullPath);
        command.ExecuteNonQuery();
    }

    public void UpdateWatchingApplicationActivity(string exeName, DateTime activityTime)
    {
        using var connection = _database.OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE WatchingExeInfo SET LastActivityAt = @Activity, UpdatedAt = @Now WHERE ExeName = @Name;";
        command.Parameters.AddWithValue("@Name", WatchingExeInfo.NormalizeProcessName(exeName));
        command.Parameters.AddWithValue("@Activity", activityTime.ToString("O"));
        command.Parameters.AddWithValue("@Now", DateTime.Now.ToString("O"));
        command.ExecuteNonQuery();
    }
}
