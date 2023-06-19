using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NetChatGPT.TelegramBotApi.Logic.Commands
{
    public class HelpCommand :  ICommandDialog
    {
        public HelpCommand() 
        {
           
        }

        public bool CheckCommand(ChatContext chatContext)
        {
            return chatContext.Text.StartsWith("/help");
        }

        public async Task Run(ChatContext chatContext)
        {
            await chatContext.BotClient.SendTextMessageAsync(chatContext.Message.Chat.Id,
                  "👋 Hi there! I'm here to assist you with any questions or conversations you have. " +
                  "Just type in your query, and I'll do my best to provide you with helpful and insightful responses. " +
                  "Feel free to ask about a wide range of topics, from general knowledge to specific interests. " +
                  "Remember, I'm an AI language model, " +
                  "so while I strive to provide accurate information, there might be limitations to my understanding. " +
                  "Let's engage in a friendly and informative conversation together",
                  parseMode: ParseMode.Html, disableNotification: chatContext.DisableNotification)
              .ConfigureAwait(false);
        }
    }

}

