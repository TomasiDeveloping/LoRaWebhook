using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok("API is running...");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}