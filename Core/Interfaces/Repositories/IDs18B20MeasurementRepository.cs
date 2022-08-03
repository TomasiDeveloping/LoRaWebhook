using Core.DataTransferObjects;

namespace Core.Interfaces.Repositories;

public interface IDs18B20MeasurementRepository
{
    Task<List<Ds18B20MeasurementDto>> GetMeasurementsAsync();
    Task<Ds18B20MeasurementDto> InsertAsync(Ds18B20MeasurementDto measurementDto);
}