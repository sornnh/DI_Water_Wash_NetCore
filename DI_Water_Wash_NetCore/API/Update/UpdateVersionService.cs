using System.Net.Http.Json;
using System.Text.Json;


public sealed class UpdateVersionService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public UpdateVersionService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<CheckVersionResponse?> CheckVersionAsync(
        CheckVersionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));

        using var response = await _httpClient.PostAsJsonAsync(
            "api/update/check-version",
            request,
            JsonOptions,
            timeout.Token);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(timeout.Token);
            throw new HttpRequestException(
                $"Update check failed with HTTP {(int)response.StatusCode} ({response.ReasonPhrase}). " +
                $"Response: {errorBody}");
        }

        return await response.Content.ReadFromJsonAsync<CheckVersionResponse>(JsonOptions, timeout.Token);
    }
}
