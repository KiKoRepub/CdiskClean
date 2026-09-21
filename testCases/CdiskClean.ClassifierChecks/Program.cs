using CdiskClean.Helpers;
using CdiskClean.Models.cleanUp;
using CdiskClean.Services;
using CdiskClean.Services.database;
using Microsoft.Data.Sqlite;

var classifier = new CleanupClassifier();
await AgentChecks.RunAsync();

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
await CheckCleanupScannerAsync();

Console.WriteLine("Classifier, database, scanner and protected-path checks passed.");

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
        database.Initialize();
        const string watchedPath = @"D:\Data\100%_cache";
        database.Rules.SaveWatchDirectory(new CdiskClean.Models.rules.WatchingDirectory(watchedPath, true));
        database.Rules.SaveWatchDirectory(new CdiskClean.Models.rules.WatchingDirectory(watchedPath, false));
        if (database.Rules.GetWatchDirectories().Single().IncludeSubdirs)
            throw new InvalidOperationException("Directory update failed.");
        database.Rules.DeleteWatchDirectory(watchedPath);
        if (database.Rules.GetWatchDirectories().Count != 0)
            throw new InvalidOperationException("Directory delete failed.");
        foreach (var file in new[] { watchedPath + @"\one.tmp", @"D:\Data\100XXcache\other.tmp" })
            database.History.SaveChangeRecord(new CdiskClean.Models.FileChangeRecord
            {
                Timestamp = DateTime.Now,
                FullPath = file,
                FileName = Path.GetFileName(file),
                Directory = Path.GetDirectoryName(file)!
            });
        if (database.History.GetChangeRecords().Count != 2 ||
            database.History.GetChangeRecordsUnderPath(watchedPath).Single().FullPath != watchedPath + @"\one.tmp")
            throw new InvalidOperationException("Path query escaping failed.");
        database.History.SaveCleanupRecord(new CleanupRecord
        {
            CleanupTime = DateTime.Now,
            FullPath = @"D:\Data\Temp\old.tmp",
            FileName = "old.tmp",
            SizeBytes = 10,
            Method = "回收站",
            Category = CleanupCategory.TemporaryFiles.GetDisplayName(),
            Success = true
        });

        var record = database.History.GetCleanupRecords(1).Single();
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

static async Task CheckCleanupScannerAsync()
{
    var root = Path.Combine(AppContext.BaseDirectory, $"scanner-check-{Guid.NewGuid():N}");
    Directory.CreateDirectory(Path.Combine(root, "child"));
    try
    {
        await File.WriteAllBytesAsync(Path.Combine(root, "one.tmp"), new byte[3]);
        await File.WriteAllBytesAsync(Path.Combine(root, "child", "two.tmp"), new byte[5]);
        var entries = await CleanupScanner.ScanDirectoryAsync(root);
        if (entries.Count != 4 || entries[0].FullPath != root || entries[0].SizeBytes != 8 ||
            entries.Single(entry => entry.Name == "child").SizeBytes != 5)
            throw new InvalidOperationException("Scanner must preserve parent-first order and aggregate sizes.");
        var file = entries.Single(entry => entry.Name == "one.tmp");
        foreach (var method in Enum.GetValues<CleanupMethod>())
        {
            if (CleanupFileOperations.Execute(file, method, null, out var error, out var freed) ||
                string.IsNullOrEmpty(error) || freed != 0 || !File.Exists(file.FullPath))
                throw new InvalidOperationException("Cleanup must refuse paths inside the application directory.");
        }
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        try
        {
            await CleanupScanner.ScanDirectoryAsync(root, cts.Token);
            throw new InvalidOperationException("A cancelled scan must not run.");
        }
        catch (OperationCanceledException) { }
    }
    finally
    {
        var fullRoot = Path.GetFullPath(root);
        if (!fullRoot.StartsWith(Path.GetFullPath(AppContext.BaseDirectory), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Test cleanup path escaped the output directory.");
        if (Directory.Exists(fullRoot)) Directory.Delete(fullRoot, true);
    }
}
