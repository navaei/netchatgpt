using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NetChatGPT.TelegramBotApi.Logic.Commands
{
    public class ImageCommand :  ICommandDialog
    {
        public ImageCommand() 
        {
           
        }

        public bool CheckCommand(ChatContext chatContext)
        {
            return chatContext.Text.StartsWith("/img");
        }

        public async Task Run(ChatContext chatContext)
        {
            //TODO
            throw new NotImplementedException();
        }
    }

}

