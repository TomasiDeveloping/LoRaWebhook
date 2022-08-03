using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;
    private readonly IWebhookService _webhookService;

    public WebhookController(IWebhookService webhookService, ILogger<WebhookController> logger)
    {
        _webhookService = webhookService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Webhook([FromBody] JObject obj)
    {
        try
        {
            await _webhookService.ProcessingSensorData(obj);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Ok();
        }
    }
}