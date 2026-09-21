using System.Security.Cryptography;
using System.Text.Json;

namespace CdiskClean.agents;

public sealed class AgentSettings
{
    public bool Enabled { get; set; }
    public string Endpoint { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 30;

    public Uri Validate()
    {
        if (!Uri.TryCreate(Endpoint, UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Query) || !string.IsNullOrEmpty(uri.Fragment))
            throw new InvalidOperationException("请填写完整 HTTPS 请求地址，不含凭据、查询参数或片段。");
        if (string.IsNullOrWhiteSpace(ApiKey) || ApiKey.Any(char.IsControl) || ApiKey.Length > 4096)
            throw new InvalidOperationException("请填写有效 API Key（不含换行）。");
        if (string.IsNullOrWhiteSpace(Model) || Model.Length > 200)
            throw new InvalidOperationException("请填写模型或 Agent 名称（最多 200 字符）。");
        if (TimeoutSeconds is < 1 or > 300)
            throw new InvalidOperationException("超时必须为 1–300 秒。");
        return uri;
    }

    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CdiskClean", "ai-settings.dat");
    public static AgentSettings Load() => Load(SettingsPath);
    internal static AgentSettings Load(string path)
    {
        if (!File.Exists(path)) return new();
        var bytes = ProtectedData.Unprotect(File.ReadAllBytes(path), null, DataProtectionScope.CurrentUser);
        try { return JsonSerializer.Deserialize<AgentSettings>(bytes) ?? throw new InvalidDataException(); }
        finally { CryptographicOperations.ZeroMemory(bytes); }
    }
    public void Save() => Save(SettingsPath);
    internal void Save(string path)
    {
        if (Enabled) Validate();
        var bytes = JsonSerializer.SerializeToUtf8Bytes(this);
        byte[] encrypted;
        try { encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser); }
        finally { CryptographicOperations.ZeroMemory(bytes); }
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temporary = path + ".tmp";
        try { File.WriteAllBytes(temporary, encrypted); File.Move(temporary, path, true); }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}
