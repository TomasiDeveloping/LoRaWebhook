using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class Bme280MeasurementController : ControllerBase
{
    private readonly IBme280MeasurementRepository _bme280MeasurementRepository;

    public Bme280MeasurementController(IBme280MeasurementRepository bme280MeasurementRepository)
    {
        _bme280MeasurementRepository = bme280MeasurementRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Bme280Measurement>>> Get()
    {
        try
        {
            var measurements = await _bme280MeasurementRepository.GetMeasurementsAsync();
            return Ok(measurements);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Bme280Measurement>> Post(Bme280MeasurementDto bme280Measurement)
    {
        try
        {
            var newMeasurement = await _bme280MeasurementRepository.InsertAsync(bme280Measurement);
            return Ok(newMeasurement);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}