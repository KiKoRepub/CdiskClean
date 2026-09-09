namespace CdiskClean.Services.database;

// Table definitions stay compatible with existing database files.
internal static class DatabaseSchema
{
    internal const string CreateTables = @"CREATE TABLE IF NOT EXISTS WatchDirectories (
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
            );
CREATE TABLE IF NOT EXISTS ProcessNotifications (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProcessName TEXT NOT NULL,
                OperationCount INTEGER NOT NULL,
                DurationSeconds INTEGER NOT NULL,
                TriggerTime TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
            );
CREATE TABLE IF NOT EXISTS IgnoreProcessRecord (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProcessName TEXT NOT NULL UNIQUE,
                Status TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
CREATE TABLE IF NOT EXISTS WatchingExeInfo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExeName TEXT NOT NULL,
                FullPath TEXT NOT NULL UNIQUE,
                SizeBytes INTEGER NULL,
                RunningState TEXT NOT NULL DEFAULT '未知',
                MonitoringState TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                LastActivityAt TEXT NULL
            );
CREATE TABLE IF NOT EXISTS CleanupRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CleanupTime TEXT NOT NULL,
                FullPath TEXT NOT NULL,
                FileName TEXT NOT NULL,
                SizeBytes INTEGER,
                Method TEXT NOT NULL,
                Category TEXT NOT NULL DEFAULT '其他',
                Success INTEGER NOT NULL,
                Message TEXT,
                CreatedAt TEXT NOT NULL
            );";
}
