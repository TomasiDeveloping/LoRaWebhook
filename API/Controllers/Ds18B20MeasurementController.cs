using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v{v:apiVersion}[controller]")]
[ApiController]
public class Ds18B20MeasurementController : ControllerBase
{
    private readonly IDs18B20MeasurementRepository _ds18B20MeasurementRepository;

    public Ds18B20MeasurementController(IDs18B20MeasurementRepository ds18B20MeasurementRepository)
    {
        _ds18B20MeasurementRepository = ds18B20MeasurementRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ds18B20MeasurementDto>>> Get()
    {
        try
        {
            var measurements = await _ds18B20MeasurementRepository.GetMeasurementsAsync();
            return Ok(measurements);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Ds18B20MeasurementDto>> Post(Ds18B20MeasurementDto ds18B20Measurement)
    {
        try
        {
            var newMeasurement = await _ds18B20MeasurementRepository.InsertAsync(ds18B20Measurement);
            return Ok(newMeasurement);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}