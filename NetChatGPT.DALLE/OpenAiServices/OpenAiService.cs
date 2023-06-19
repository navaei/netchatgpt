using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace NetChatGPT.DALLE.OpenAiServices;

public class OpenAiService : IOpenAIProxy
{
    readonly HttpClient _httpClient;
    readonly string _subscriptionId;
    readonly string _apiKey;

    public OpenAiService(IConfiguration configuration)
    {
        var openApiUrl = configuration["OpenAi:Url"] ?? throw new ArgumentException(nameof(configuration));
        _httpClient = new HttpClient { BaseAddress = new Uri(openApiUrl) };

        _subscriptionId = configuration["OpenAi:SubscriptionId"];
        _apiKey = configuration["OpenAi:ApiKey"];
    }

    public async Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default)
    {
        using var httpreq = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");

        var jsonRequest = JsonSerializer.Serialize(prompt, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        httpreq.Content = new StringContent(jsonRequest);
        httpreq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        httpreq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        httpreq.Headers.TryAddWithoutValidation("OpenAI-Organization", _subscriptionId);

        var response = await _httpClient.SendAsync(httpreq, 
            HttpCompletionOption.ResponseHeadersRead, cancellation);
        response.EnsureSuccessStatusCode();
        var content = response.Content;
        var jsonResponse = await content.ReadFromJsonAsync<GenerateImageResponse>(cancellationToken: cancellation);
        return jsonResponse;
    }

    public async Task<byte[]> DownloadImage(string url)
    {
        var buffer = await _httpClient.GetByteArrayAsync(url);
        return buffer;
    }
}
