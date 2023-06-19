using Microsoft.AspNetCore.Mvc;
using NetChatGPT.TelegramBotApi.Logic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NetChatGPT.TelegramBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly ILogger<UpdateController> _logger;
        private readonly IMainApp _mainApp;

        public UpdateController(ILogger<UpdateController> logger, IMainApp mainApp)
        {
            _logger = logger;
            _mainApp = mainApp;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        {
            try
            {
                await _mainApp.ThinkAsync(update)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Proccess Update");
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

    }
}
