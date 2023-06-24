using System.Text.Json;
using System.Text;
using NetChatGPT.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var config = BuildConfig();
var subscriptionId = config["OpenAi:SubscriptionId"];
var chatGptApiKey = config["OpenAi:ApiKey"];
var openApiUrl = config["OpenAi:Url"];

var request = new ChatGPTRequest
{
    Model = "text-davinci-003",
    MaxTokens = 120
};

GetPrompt:
Console.WriteLine("Type your prompt:");
request.Prompt = Console.ReadLine();
if (string.IsNullOrEmpty(request.Prompt) || request.Prompt.Length < 5)
{
    Console.WriteLine("Finish");
    Environment.Exit(1);
}

var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/completions");
httpReq.Headers.Add("Authorization", $"Bearer {chatGptApiKey}");
httpReq.Headers.TryAddWithoutValidation("OpenAI-Organization", subscriptionId);
string requestString = JsonSerializer.Serialize(request);
httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

var httpClient = new HttpClient { BaseAddress = new Uri(openApiUrl) };
var httpResponse = await httpClient.SendAsync(httpReq);
var responseString = await httpResponse.Content.ReadAsStringAsync();
var response = JsonSerializer.Deserialize<ChatGPTResponse>(responseString);

if (response.Error != null)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(response.Error.Message);
    Console.ForegroundColor = ConsoleColor.Gray;
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.Choices.FirstOrDefault()?.Text + " \n");
    Console.ForegroundColor = ConsoleColor.Gray;
}

goto GetPrompt;


static IConfiguration BuildConfig()
{
    var dir = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    return configBuilder.Build();
}
