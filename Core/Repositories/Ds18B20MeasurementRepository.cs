using AutoMapper;
using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class Ds18B20MeasurementRepository : IDs18B20MeasurementRepository
{
    private readonly LoRaContext _loRaContext;
    private readonly IMapper _mapper;

    public Ds18B20MeasurementRepository(LoRaContext loRaContext, IMapper mapper)
    {
        _loRaContext = loRaContext;
        _mapper = mapper;
    }

    public async Task<List<Ds18B20MeasurementDto>> GetMeasurementsAsync()
    {
        var measurements = await _loRaContext.Ds18B20Measurements
            .Include(m => m.Sensor)
            .OrderByDescending(m => m.SendTime)
            .AsNoTracking()
            .ToListAsync();
        return measurements.Any()
            ? _mapper.Map<List<Ds18B20MeasurementDto>>(measurements)
            : new List<Ds18B20MeasurementDto>();
    }

    public async Task<Ds18B20MeasurementDto> InsertAsync(Ds18B20MeasurementDto measurementDto)
    {
        var measurement = _mapper.Map<Ds18B20Measurement>(measurementDto);
        await _loRaContext.Ds18B20Measurements.AddAsync(measurement);
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<Ds18B20MeasurementDto>(measurement);
    }
}