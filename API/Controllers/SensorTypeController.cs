using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class SensorTypeController : ControllerBase
{
    private readonly ISensorTypeRepository _sensorTypeRepository;

    public SensorTypeController(ISensorTypeRepository sensorTypeRepository)
    {
        _sensorTypeRepository = sensorTypeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<SensorTypeDto>>> Get()
    {
        try
        {
            var sensorTypes = await _sensorTypeRepository.GetSensorTypesAsync();
            if (!sensorTypes.Any()) return NoContent();
            return Ok(sensorTypes);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("sensorId")]
    public async Task<ActionResult<SensorTypeDto>> Get(Guid sensorId)
    {
        try
        {
            var sensorType = await _sensorTypeRepository.GetSensorTypeBySensorIdAsync(sensorId);
            if (sensorType == null) return NotFound($"No sensor type found with sensorId: {sensorId}");
            return Ok(sensorType);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<SensorTypeDto>> Post(SensorTypeDto sensorTypeDto)
    {
        try
        {
            var newSensor = await _sensorTypeRepository.InsertAsync(sensorTypeDto);
            return Ok(newSensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("sensorTypeId")]
    public async Task<ActionResult<SensorTypeDto>> Put(Guid sensorTypeId, SensorTypeDto sensorTypeDto)
    {
        try
        {
            if (!sensorTypeId.Equals(sensorTypeDto.Id)) return BadRequest("Ids are not equals!");
            var updatesSensorType = await _sensorTypeRepository.UpdateAsync(sensorTypeDto);
            return Ok(updatesSensorType);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("sensorTypeId")]
    public async Task<IActionResult> Delete(Guid sensorTypeId)
    {
        try
        {
            var checkDelete = await _sensorTypeRepository.DeleteAsync(sensorTypeId);
            return checkDelete ? Ok() : BadRequest($"SensorType with id: {sensorTypeId} could not deleted!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}