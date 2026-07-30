using Microsoft.Data.Sqlite;

namespace CdiskClean.Helpers;

public static class DBHelpers
{
    public static SqliteConnection CreateConnection(string dbPath)
    {
        var csb = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true
        };
        var conn = new SqliteConnection(csb.ToString());
        conn.Open();

        using var pragma = conn.CreateCommand();
        pragma.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=3000;";
        pragma.ExecuteNonQuery();

        return conn;
    }

    public static int ExecuteNonQuery(string dbPath, string sql, params (string Name, object? Value)[] parameters)
    {
        using var conn = CreateConnection(dbPath);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in parameters)
            cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return cmd.ExecuteNonQuery();
    }

    public static int ExecuteNonQuery(SqliteConnection conn, SqliteTransaction? transaction, string sql, params (string Name, object? Value)[] parameters)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = sql;
        foreach (var (name, value) in parameters)
            cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return cmd.ExecuteNonQuery();
    }

    public static T? ExecuteScalar<T>(string dbPath, string sql, params (string Name, object? Value)[] parameters)
    {
        using var conn = CreateConnection(dbPath);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in parameters)
            cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);

        var result = cmd.ExecuteScalar();
        return result is DBNull or null ? default : (T)Convert.ChangeType(result, typeof(T));
    }

    public static List<T> Query<T>(string dbPath, string sql, Func<SqliteDataReader, T> mapper, params (string Name, object? Value)[] parameters)
    {
        var list = new List<T>();
        using var conn = CreateConnection(dbPath);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (name, value) in parameters)
            cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(mapper(reader));
        return list;
    }

    public static bool TableExists(string dbPath, string tableName)
    {
        return ExecuteScalar<long>(dbPath,
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@Name;",
            ("@Name", tableName)) > 0;
    }

    public static void CreateTableIfNotExists(string dbPath, string tableName, string columnsDefinition)
    {
        ExecuteNonQuery(dbPath, $"CREATE TABLE IF NOT EXISTS [{tableName}] ({columnsDefinition});");
    }

    public static void ExecuteInTransaction(string dbPath, Action<SqliteConnection, SqliteTransaction> action)
    {
        using var conn = CreateConnection(dbPath);
        using var tx = conn.BeginTransaction();
        try
        {
            action(conn, tx);
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public static void Vacuum(string dbPath)
    {
        ExecuteNonQuery(dbPath, "VACUUM;");
    }

    /// <summary>
    /// 安全获取 string 值, DbNull 返回 null
    /// </summary>
    public static string? GetStringOrNull(this SqliteDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);

    /// <summary>
    /// 安全获取 long? 值, DbNull 返回 null
    /// </summary>
    public static long? GetInt64OrNull(this SqliteDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetInt64(ordinal);

    /// <summary>
    /// 安全获取 DateTime 值, DbNull 返回 default
    /// </summary>
    public static DateTime GetDateTimeOrDefault(this SqliteDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? default : reader.GetDateTime(ordinal);

    public static long GetLastInsertRowId(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT last_insert_rowid();";
        return (long)cmd.ExecuteScalar()!;
    }

    public static long CountRows(string dbPath, string tableName, string? whereClause = null, params (string Name, object? Value)[] parameters)
    {
        var sql = $"SELECT COUNT(*) FROM [{tableName}]";
        if (!string.IsNullOrWhiteSpace(whereClause))
            sql += $" WHERE {whereClause}";
        return ExecuteScalar<long>(dbPath, sql, parameters);
    }
}
