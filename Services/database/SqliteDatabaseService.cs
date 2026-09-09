using Microsoft.Data.Sqlite;

namespace CdiskClean.Services.database;

public sealed class SqliteDatabaseService
{
    private readonly string _connectionString;
    public MonitoringRuleStore Rules { get; }
    public HistoryStore History { get; }

    public SqliteDatabaseService(string dbPath)
    {
        Rules = new MonitoringRuleStore(this);
        History = new HistoryStore(this);
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
        using var connection = OpenConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA journal_mode=WAL;" + DatabaseSchema.CreateTables;

        cmd.ExecuteNonQuery();
        EnsureWatchingApplicationColumns(connection);
        EnsureCleanupRecordColumns(connection);
        TrimHistoryTables(connection);
    }

    private static void EnsureCleanupRecordColumns(SqliteConnection connection)
    {
        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using (var query = connection.CreateCommand())
        {
            query.CommandText = "PRAGMA table_info(CleanupRecords);";
            using var reader = query.ExecuteReader();
            while (reader.Read()) columns.Add(reader.GetString(1));
        }

        if (columns.Contains("Category")) return;
        using var command = connection.CreateCommand();
        command.CommandText = "ALTER TABLE CleanupRecords ADD COLUMN Category TEXT NOT NULL DEFAULT '其他';";
        command.ExecuteNonQuery();
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

    internal SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }
}
