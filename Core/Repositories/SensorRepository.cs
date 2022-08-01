using AutoMapper;
using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly LoRaContext _loRaContext;
    private readonly IMapper _mapper;

    public SensorRepository(LoRaContext loRaContext, IMapper mapper)
    {
        _loRaContext = loRaContext;
        _mapper = mapper;
    }

    public async Task<List<SensorDto>> GetSensorsAsync()
    {
        var sensors = await _loRaContext.Sensors
            .Include(s => s.SensorType)
            .AsNoTracking()
            .ToListAsync();
        return sensors.Any() ? _mapper.Map<List<SensorDto>>(sensors) : new List<SensorDto>();
    }

    public async Task<SensorDto?> GetSensorByDevAddressAsync(string devAddress)
    {
        var sensor = await _loRaContext.Sensors
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.DevAddress.Equals(devAddress));
        return sensor == null ? null : _mapper.Map<SensorDto>(sensor);
    }

    public async Task<SensorDto> InsertAsync(SensorDto sensorDto)
    {
        var sensor = _mapper.Map<Sensor>(sensorDto);
        await _loRaContext.Sensors.AddAsync(sensor);
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<SensorDto> UpdateAsync(SensorDto sensorDto)
    {
        var sensor = await _loRaContext.Sensors.FirstOrDefaultAsync(s => s.Id.Equals(sensorDto.Id));
        if (sensor == null) throw new ArgumentException($"No sensor found with id: {sensorDto.Id}");
        _mapper.Map(sensorDto, sensor);
        sensor.UpdatedAt = DateTime.Now;
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<bool> DeleteAsync(Guid sensorGuid)
    {
        var sensor = await _loRaContext.Sensors.FirstOrDefaultAsync(s => s.Id.Equals(sensorGuid));
        if (sensor == null) return false;
        _loRaContext.Sensors.Remove(sensor);
        await _loRaContext.SaveChangesAsync();
        return true;
    }
}