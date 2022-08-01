using AutoMapper;
using Core.DataTransferObjects;
using Core.Interfaces.Repositories;
using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class SensorTypeRepository : ISensorTypeRepository
{
    private readonly LoRaContext _loRaContext;
    private readonly IMapper _mapper;

    public SensorTypeRepository(LoRaContext loRaContext, IMapper mapper)
    {
        _loRaContext = loRaContext;
        _mapper = mapper;
    }

    public async Task<List<SensorTypeDto>> GetSensorTypesAsync()
    {
        var sensorTypes = await _loRaContext.SensorTypes
            .AsNoTracking()
            .ToListAsync();
        return sensorTypes.Any() ? _mapper.Map<List<SensorTypeDto>>(sensorTypes) : new List<SensorTypeDto>();
    }

    public async Task<SensorTypeDto?> GetSensorTypeBySensorIdAsync(Guid sensorId)
    {
        var sensorType = await _loRaContext.SensorTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id.Equals(sensorId));
        return sensorType == null ? null : _mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<SensorTypeDto> InsertAsync(SensorTypeDto sensorTypeDto)
    {
        var sensorType = _mapper.Map<SensorType>(sensorTypeDto);
        await _loRaContext.SensorTypes.AddAsync(sensorType);
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<SensorTypeDto> UpdateAsync(SensorTypeDto sensorTypeDto)
    {
        var sensorType = await _loRaContext.SensorTypes.FirstOrDefaultAsync(st => st.Id.Equals(sensorTypeDto.Id));
        if (sensorType == null) throw new ArgumentException($"No sensor type found with id: {sensorTypeDto.Id}");
        _mapper.Map(sensorTypeDto, sensorType);
        sensorType.UpdatedAt = DateTime.Now;
        await _loRaContext.SaveChangesAsync();
        return _mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<bool> DeleteAsync(Guid sensorId)
    {
        var sensorType = await _loRaContext.SensorTypes.FirstOrDefaultAsync(st => st.Id.Equals(sensorId));
        if (sensorType == null) return false;
        _loRaContext.SensorTypes.Remove(sensorType);
        await _loRaContext.SaveChangesAsync();
        return true;
    }
}