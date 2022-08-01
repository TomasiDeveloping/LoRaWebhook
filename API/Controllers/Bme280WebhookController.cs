using Core.Interfaces.Decoders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class Bme280WebhookController : ControllerBase
{
    private readonly IBme280Decoder _bme280Decoder;
    private readonly ILogger<Bme280WebhookController> _logger;

    public Bme280WebhookController(IBme280Decoder bme280Decoder, ILogger<Bme280WebhookController> logger)
    {
        _bme280Decoder = bme280Decoder;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Webhook([FromBody] JObject obj)
    {
        try
        {
            await _bme280Decoder.CreateMeasurement(obj);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Ok();
        }

    }
}