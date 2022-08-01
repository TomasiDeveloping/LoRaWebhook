using Core.DataTransferObjects;

namespace Core.Interfaces.Repositories;

public interface ISensorRepository
{
    Task<List<SensorDto>> GetSensorsAsync();
    Task<SensorDto?> GetSensorByDevAddressAsync(string devAddress);
    Task<SensorDto> InsertAsync(SensorDto sensorDto);
    Task<SensorDto> UpdateAsync(SensorDto sensorDto);
    Task<bool> DeleteAsync(Guid sensorGuid);
}