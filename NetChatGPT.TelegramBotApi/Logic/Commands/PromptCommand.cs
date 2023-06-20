using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System;
using NetChatGPT.TelegramBotApi.Logic;
using System.Text;
using NetChatGPT.Common;
using System.Text.Json;

namespace NetChatGPT.TelegramBotApi.Logic.Commands
{
    public class PromptCommand : ICommandDialog
    {
        public PromptCommand()
        {

        }

        public bool CheckCommand(ChatContext chatContext)
        {
            return chatContext.Text.Length > 5;
        }

        public async Task Run(ChatContext chatContext)
        {
            var response = await GetChatGptResponse(chatContext.AppConfig, chatContext.Text);
            if (string.IsNullOrEmpty(response))
            {
                await chatContext.BotClient.SendTextMessageAsync(chatContext.Message.Chat.Id, "Response invalid",
                   parseMode: ParseMode.Html, disableNotification: chatContext.DisableNotification)
               .ConfigureAwait(false);
                return;
            }

            response = response.Replace("\n\n", "");
            await chatContext.BotClient.SendTextMessageAsync(chatContext.Message.Chat.Id, response,
                parseMode: ParseMode.Html, disableNotification: chatContext.DisableNotification)
            .ConfigureAwait(false);
        }

        static async Task<string> GetChatGptResponse(AppConfig appConfig, string prompt)
        {
            var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/completions");
            httpReq.Headers.Add("Authorization", $"Bearer {appConfig.OpenAiApiKey}");
            httpReq.Headers.TryAddWithoutValidation("OpenAI-Organization", appConfig.SubscriptionId);
            string requestString = JsonSerializer.Serialize(new ChatGPTRequest
            {
                Model = "text-davinci-003",
                MaxTokens = 120,
                Prompt = prompt
            });
            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient { BaseAddress = new Uri(appConfig.OpenAiUrl) };
            var httpResponse = await httpClient.SendAsync(httpReq);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ChatGPTResponse>(responseString);
            var chatGptResponse = response.Choices.FirstOrDefault()?.Text;
            return chatGptResponse;
        }
    }
}

