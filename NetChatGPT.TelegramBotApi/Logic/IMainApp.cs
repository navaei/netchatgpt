using Telegram.Bot;
using Update = Telegram.Bot.Types.Update;

namespace NetChatGPT.TelegramBotApi.Logic
{
    public interface IMainApp
    {
        Task ThinkAsync(Update update);
        string TestStatus();
    }

    public interface ICommandDialog
    {
        bool CheckCommand(ChatContext chatContext);
        Task Run(ChatContext chatContext);
    }
}
