using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class SensorController : ControllerBase
{
    private readonly ISensorRepository _sensorRepository;

    public SensorController(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<SensorDto>>> Get()
    {
        try
        {
            var sensors = await _sensorRepository.GetSensorsAsync();
            return Ok(sensors);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{devAddress}")]
    public async Task<ActionResult<SensorDto>> Get(string devAddress)
    {
        try
        {
            var sensor = await _sensorRepository.GetSensorByDevAddressAsync(devAddress);
            if (sensor == null) return NotFound($"No sensor found with devAddress: {devAddress}");
            return Ok(sensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<SensorDto>> Post(SensorDto sensor)
    {
        try
        {
            var newSensor = await _sensorRepository.InsertAsync(sensor);
            return Ok(newSensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{sensorId}")]
    public async Task<ActionResult<SensorDto>> Put(Guid sensorId, SensorDto sensor)
    {
        try
        {
            if (sensorId != sensor.Id) return BadRequest("Sensor ids not match!");
            var updatedSensor = await _sensorRepository.UpdateAsync(sensor);
            return Ok(updatedSensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("sensorId")]
    public async Task<IActionResult> Delete(Guid sensorId)
    {
        try
        {
            var checkDelete = await _sensorRepository.DeleteAsync(sensorId);
            if (!checkDelete) return BadRequest("Could not delete sensor!");
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}