using AutoMapper;
using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class Bme280MeasurementRepository : IBme280MeasurementRepository
{
    private readonly LoRaContext _loRaContext;
    private readonly IMapper _mapper;

    public Bme280MeasurementRepository(LoRaContext loRaContext, IMapper mapper)
    {
        _loRaContext = loRaContext;
        _mapper = mapper;
    }

    public async Task<List<Bme280MeasurementDto>> GetMeasurementsAsync()
    {
        var measurements = await _loRaContext.Bme280Measurements
            .Include(m => m.Sensor)
            .OrderByDescending(m => m.SendTime)
            .AsNoTracking()
            .ToListAsync();
        return measurements.Any()
            ? _mapper.Map<List<Bme280MeasurementDto>>(measurements)
            : new List<Bme280MeasurementDto>();
    }

    public async Task<Bme280MeasurementDto> InsertAsync(Bme280MeasurementDto measurementDto)
    {
        var measurement = _mapper.Map<Bme280Measurement>(measurementDto);
        await _loRaContext.Bme280Measurements.AddAsync(measurement);
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<Bme280MeasurementDto>(measurement);
    }
}