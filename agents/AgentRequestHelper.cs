using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
namespace CdiskClean.agents;

public static class AgentRequestHelper
{
    private static readonly HttpClient Client = new(new HttpClientHandler { AllowAutoRedirect = false })
    { Timeout = Timeout.InfiniteTimeSpan };
    public static Task<T> sendAgentRequest<T>(string prompt, object? data = null, string? returnFormat = null,
        CancellationToken cancellationToken = default) => SendAsync<T>(AgentSettings.Load(), prompt, data, returnFormat, cancellationToken);
    public static Task<T> SendAsync<T>(AgentSettings settings, string prompt, object? data = null,
        string? returnFormat = null, CancellationToken cancellationToken = default) =>
        SendAsync<T>(Client, settings, prompt, data, returnFormat, cancellationToken);
    internal static async Task<T> SendAsync<T>(HttpClient client, AgentSettings settings, string prompt,
        object? data = null, string? returnFormat = null, CancellationToken cancellationToken = default)
    {
        if (!settings.Enabled) throw new InvalidOperationException("AI 服务未启用。");
        var endpoint = settings.Validate();
        if (string.IsNullOrWhiteSpace(prompt)) throw new InvalidOperationException("提示词不能为空。");
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(settings.TimeoutSeconds));
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        request.Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(new
        { model = settings.Model, data, prompt, returnFormat }));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        try
        {
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"AI 服务返回 HTTP {(int)response.StatusCode}，请检查配置或稍后重试。");
            const int limit = 1024 * 1024;
            if (response.Content.Headers.ContentLength > limit) throw new InvalidOperationException("AI 响应超过 1 MiB 上限。");
            await using var stream = await response.Content.ReadAsStreamAsync(timeout.Token).ConfigureAwait(false);
            using var buffer = new MemoryStream();
            var chunk = new byte[8192];
            int count;
            while ((count = await stream.ReadAsync(chunk, timeout.Token).ConfigureAwait(false)) > 0)
            {
                if (buffer.Length + count > limit) throw new InvalidOperationException("AI 响应超过 1 MiB 上限。");
                buffer.Write(chunk, 0, count);
            }
            return JsonSerializer.Deserialize<T>(buffer.ToArray()) ?? throw new InvalidOperationException("AI 响应为空。");
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        { throw new InvalidOperationException("AI 请求超时。"); }
        catch (HttpRequestException) { throw new InvalidOperationException("无法连接 AI 服务，请检查网络和服务地址。"); }
        catch (JsonException) { throw new InvalidOperationException("AI 响应不是约定的 JSON 格式。"); }
        catch (IOException) { throw new InvalidOperationException("AI 响应读取失败。"); }
    }
}
