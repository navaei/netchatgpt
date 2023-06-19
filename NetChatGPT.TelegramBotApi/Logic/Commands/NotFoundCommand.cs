using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace NetChatGPT.TelegramBotApi.Logic.Commands
{
    public class NotFoundCommand : ICommandDialog
    {
        public NotFoundCommand()
        {

        }

        public bool CheckCommand(ChatContext chatContext)
        {
            return true;
        }

        public async Task Run(ChatContext chatContext)
        {
            await chatContext.BotClient.SendTextMessageAsync(chatContext.Message.Chat.Id,
                "Prompt Not Found!",
                parseMode: ParseMode.Html, disableNotification: chatContext.DisableNotification)
            .ConfigureAwait(false);
        }
    }
}

