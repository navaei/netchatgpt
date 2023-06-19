using System.Text.Json;
using System.Text;
using NetChatGPT.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

var config = BuildConfig();
var subscriptionId = config["OpenAi:SubscriptionId"];
var openApiKey = config["OpenAi:ApiKey"];
var openApiUrl = config["OpenAi:Url"];

Console.WriteLine("Bot started...");
var botClient = new TelegramBotClient(config["Telegram:BotToken"]);

var lastUpdate = 0;

while (true)
{
    var updates = botClient.GetUpdatesAsync(offset: lastUpdate).Result;
    if (updates.Any())
    {
        foreach (var update in updates)
        {
            if (update.Message.Text != null)
            {
                string userInput = update.Message.Text;
                string chatId = update.Message.Chat.Id.ToString();

                // Send user input to ChatGPT API
                string chatGptResponse = GetChatGptResponse(openApiUrl, openApiKey, subscriptionId, userInput).Result;

                // Send ChatGPT response to Telegram
                botClient.SendTextMessageAsync(chatId, chatGptResponse).Wait();
            }

            lastUpdate = update.Id + 1;
        }
    }

    Task.Delay(500).Wait();
}

static async Task<string> GetChatGptResponse(string openApiUrl,
    string chatGptApiKey, string subscriptionId, string prompt)
{
    var request = new ChatGPTRequest
    {
        Model = "text-davinci-003",
        MaxTokens = 120
    };
    var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/completions");
    httpReq.Headers.Add("Authorization", $"Bearer {chatGptApiKey}");
    httpReq.Headers.TryAddWithoutValidation("OpenAI-Organization", subscriptionId);
    string requestString = JsonSerializer.Serialize(new ChatGPTRequest
    {
        Model = "text-davinci-003",
        MaxTokens = 120,
        Prompt = prompt
    });
    httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

    var httpClient = new HttpClient { BaseAddress = new Uri(openApiUrl) };
    var httpResponse = await httpClient.SendAsync(httpReq);
    var responseString = await httpResponse.Content.ReadAsStringAsync();
    var response = JsonSerializer.Deserialize<ChatGPTResponse>(responseString);
    var chatGptResponse = response.Choices.FirstOrDefault()?.Text;
    return chatGptResponse;
}


static IConfiguration BuildConfig()
{
    var dir = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    return configBuilder.Build();
}
