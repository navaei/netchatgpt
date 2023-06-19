using Telegram.Bot;
using Telegram.Bot.Types;

namespace NetChatGPT.TelegramBotApi.Logic
{
    public class ChatContext
    {

        public ChatContext(Message message, TelegramBotClient botClient, AppConfig appConfig)
        {
            Message = message;
            BotClient = botClient;
            AppConfig = appConfig;
        }

        public Message Message { get; private set; }
        public AppConfig AppConfig { get; private set; }
        public string Text => Message.Text;
        public TelegramBotClient BotClient { get; private set; }
        public bool DisableNotification { get; set; }
    }
}
