namespace Core.DataTransferObjects;

public class Ds18B20MeasurementDto
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public string DevAddress { get; set; } = null!;
    public decimal Temperature { get; set; }
    public DateTime SendTime { get; set; }
}