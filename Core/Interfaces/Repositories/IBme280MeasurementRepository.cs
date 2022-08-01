using Core.DataTransferObjects;

namespace Core.Interfaces.Repositories;

public interface IBme280MeasurementRepository
{
    Task<List<Bme280MeasurementDto>> GetMeasurementsAsync();
    Task<Bme280MeasurementDto> InsertAsync(Bme280MeasurementDto measurementDto);
}