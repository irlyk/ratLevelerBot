using RatLevelerBot.Filters;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using RatLevelerBot.Services;

namespace RatLevelerBot.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
    [FromBody] Update update,
        [FromServices] RatMessageHandler handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}
