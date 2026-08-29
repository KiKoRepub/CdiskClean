using CdiskClean.Helpers;
using CdiskClean.Models.cleanUp;
using CdiskClean.Services;
using CdiskClean.Services.database;
using Microsoft.Data.Sqlite;

var classifier = new CleanupClassifier();

AssertCategory(@"D:\Data\Temp\work.tmp", CleanupCategory.TemporaryFiles);
AssertCategory(@"D:\Data\Cache\asset.bin", CleanupCategory.Cache);
AssertCategory(@"D:\Data\Logs\app.log", CleanupCategory.Logs);
AssertCategory(@"D:\Data\CrashDumps\app.dmp", CleanupCategory.CrashDumps);
AssertCategory(@"D:\Downloads\setup.msi", CleanupCategory.Installers);
AssertCategory(@"D:\Data\notes.txt", CleanupCategory.Other);

var protectedCandidate = classifier.Classify(new CleanupFileEntry
{
    FullPath = @"C:\Windows\Temp\danger.tmp",
    Name = "danger.tmp"
});
if (protectedCandidate.RiskLevel != RiskLevel.High)
    throw new InvalidOperationException("系统目录中的候选项必须保持高风险。 ");

var duplicates = classifier.Classify(new[]
{
    Entry(@"D:\Data\Cache\same.cache"),
    Entry(@"D:\Data\Cache\same.cache")
});
if (duplicates.Count != 1)
    throw new InvalidOperationException("分类器必须按路径去重。 ");

CheckCleanupCategoryMigration();

Console.WriteLine("CleanupClassifier and database migration checks passed.");

void AssertCategory(string path, CleanupCategory expected)
{
    var actual = classifier.Classify(Entry(path)).Category;
    if (actual != expected)
        throw new InvalidOperationException($"{path}: expected {expected}, actual {actual}");
}

static CleanupFileEntry Entry(string path) => new()
{
    FullPath = path,
    Name = Path.GetFileName(path)
};

static void CheckCleanupCategoryMigration()
{
    var databasePath = Path.Combine(AppContext.BaseDirectory, $"cleanup-migration-{Guid.NewGuid():N}.db");
    try
    {
        using (var connection = new SqliteConnection($"Data Source={databasePath}"))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE CleanupRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CleanupTime TEXT NOT NULL,
                    FullPath TEXT NOT NULL,
                    FileName TEXT NOT NULL,
                    SizeBytes INTEGER,
                    Method TEXT NOT NULL,
                    Success INTEGER NOT NULL,
                    Message TEXT,
                    CreatedAt TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
        }

        var database = new SqliteDatabaseService(databasePath);
        database.Initialize();
        database.SaveCleanupRecord(new CleanupRecord
        {
            CleanupTime = DateTime.Now,
            FullPath = @"D:\Data\Temp\old.tmp",
            FileName = "old.tmp",
            SizeBytes = 10,
            Method = "回收站",
            Category = CleanupCategory.TemporaryFiles.GetDisplayName(),
            Success = true
        });

        var record = database.GetCleanupRecords(1).Single();
        if (record.Category != CleanupCategory.TemporaryFiles.GetDisplayName())
            throw new InvalidOperationException("旧数据库未正确迁移清理分类列。 ");
    }
    finally
    {
        SqliteConnection.ClearAllPools();
        foreach (var suffix in new[] { string.Empty, "-wal", "-shm" })
        {
            var filePath = databasePath + suffix;
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}
