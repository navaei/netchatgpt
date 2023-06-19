using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace NetChatGPT.TelegramBotApi.Logic.Commands
{
    public class StartCommand : ICommandDialog
    {
        public StartCommand()
        {

        }

        public bool CheckCommand(ChatContext chatContext)
        {
            return chatContext.Text.StartsWith("/start") || chatContext.Text.StartsWith("/new");
        }

        public async Task Run(ChatContext chatContext)
        {
            await chatContext.BotClient.SendTextMessageAsync(chatContext.Message.Chat.Id,
                "👋 Hello! I'm your friendly chatbot ready to chat and assist you. " +
                "Just type in your questions or topics of interest, " +
                "and I'll do my best to provide you with informative and engaging responses.",
                parseMode: ParseMode.Html, disableNotification: chatContext.DisableNotification)
            .ConfigureAwait(false);
        }
    }
}

