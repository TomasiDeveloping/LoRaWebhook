namespace Core.DataTransferObjects;

public class Bme280MeasurementDto
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public string DevAddress { get; set; } = null!;
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal Pressure { get; set; }
    public DateTime SendTime { get; set; }
}