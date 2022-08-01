using Core.DataTransferObjects;

namespace Core.Interfaces.Repositories;

public interface ISensorTypeRepository
{
    Task<List<SensorTypeDto>> GetSensorTypesAsync();
    Task<SensorTypeDto?> GetSensorTypeBySensorIdAsync(Guid sensorId);
    Task<SensorTypeDto> InsertAsync(SensorTypeDto sensorTypeDto);
    Task<SensorTypeDto> UpdateAsync(SensorTypeDto sensorTypeDto);
    Task<bool> DeleteAsync(Guid sensorId);
}