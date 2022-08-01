using Core.Interfaces.Decoders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class Bme280WebhookController : ControllerBase
{
    private readonly IBme280Decoder _bme280Decoder;

    public Bme280WebhookController(IBme280Decoder bme280Decoder)
    {
        _bme280Decoder = bme280Decoder;
    }

    [HttpPost]
    public async Task<IActionResult> Webhook([FromBody] JObject obj)
    {
        await _bme280Decoder.CreateMeasurement(obj);
        return Ok();
    }
}