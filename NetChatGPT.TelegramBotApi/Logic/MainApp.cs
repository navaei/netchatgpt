using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace NetChatGPT.TelegramBotApi.Logic
{
    public class MainApp : IMainApp
    {
        private readonly AppConfig _appConfig;
        private readonly TelegramBotClient _botClient;
        private readonly List<ICommandDialog> Commands;

        public MainApp(AppConfig appConfig, TelegramBotClient botClient)
        {
            Commands = new List<ICommandDialog>()
            {
                 new Commands.StartCommand(),
                 new Commands.HelpCommand(),
                 new Commands.PromptCommand(),
                 new Commands.NotFoundCommand(),
            };

            _appConfig = appConfig;
            _botClient = botClient;
        }

        public async Task ThinkAsync(Update update)
        {
            if (update.Message == null || update.Type == UpdateType.ChannelPost)
                return;

            try
            {
                var context = new ChatContext(update.Message, _botClient, _appConfig);
                await Commands.First(c => c.CheckCommand(context)).Run(context);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public string TestStatus()
        {
            return "Well done!";
        }
    }
}
