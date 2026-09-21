using CdiskClean.agents;
using System.Net;
using System.Net.Http;
using System.Text.Json;
internal static class AgentChecks
{
    public static async Task RunAsync()
    {
        var settings = new AgentSettings { Enabled = true, Endpoint = "https://example.invalid/agent", ApiKey = "test-secret", Model = "test" };
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");
        try
        {
            settings.Save(path);
            if (System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(path)).Contains(settings.ApiKey)) throw new Exception("Plaintext credential");
            if (AgentSettings.Load(path).ApiKey != settings.ApiKey) throw new Exception("Credential round trip");
        }
        finally { if (File.Exists(path)) File.Delete(path); }
        using var client = new HttpClient(new Handler());
        var result = await AgentRequestHelper.SendAsync<JsonElement>(client, settings, "ok");
        if (!result.GetProperty("ok").GetBoolean()) throw new Exception("Response mismatch");
        foreach (var prompt in new[] { "invalid", "large", "unauthorized" })
            await Reject(() => AgentRequestHelper.SendAsync<JsonElement>(client, settings, prompt));
        settings.Enabled = false;
        await Reject(() => AgentRequestHelper.SendAsync<JsonElement>(client, settings, "ok"));
        settings.Enabled = true;
        settings.Endpoint = "http://example.invalid";
        await Reject(() => AgentRequestHelper.SendAsync<JsonElement>(client, settings, "ok"));
        settings.Endpoint = "https://example.invalid/agent";
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        try { await AgentRequestHelper.SendAsync<JsonElement>(client, settings, "wait", cancellationToken: cts.Token); throw new Exception("Cancellation ignored"); }
        catch (OperationCanceledException) { }
        settings.TimeoutSeconds = 1;
        await Reject(() => AgentRequestHelper.SendAsync<JsonElement>(client, settings, "wait"));
        Console.WriteLine("AI configuration, encryption, request, error, size, cancellation and timeout checks passed.");
    }
    private static async Task Reject(Func<Task<JsonElement>> action)
    {
        try { await action(); }
        catch (InvalidOperationException ex)
        {
            if (ex.ToString().Contains("test-secret")) throw new Exception("Credential leaked");
            return;
        }
        throw new Exception("Expected rejection");
    }
    private sealed class Handler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Post || request.Headers.Authorization?.ToString() != "Bearer test-secret") throw new Exception("Request contract");
            using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(cancellationToken));
            if (body.RootElement.GetProperty("model").GetString() != "test" || body.RootElement.GetProperty("data").ValueKind != JsonValueKind.Null) throw new Exception("Unexpected payload");
            var prompt = body.RootElement.GetProperty("prompt").GetString();
            if (prompt == "wait") await Task.Delay(Timeout.Infinite, cancellationToken);
            return new HttpResponseMessage(prompt == "unauthorized" ? HttpStatusCode.Unauthorized : HttpStatusCode.OK)
            {
                Content = new StringContent(prompt switch
                {
                    "invalid" => "not-json test-secret",
                    "large" => new string('x', 1024 * 1024 + 1),
                    "unauthorized" => "test-secret",
                    _ => "{\"ok\":true}"
                })
            };
        }
    }
}
