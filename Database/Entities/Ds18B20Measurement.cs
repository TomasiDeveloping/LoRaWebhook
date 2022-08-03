namespace Database.Entities;

public class Ds18B20Measurement
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public Sensor Sensor { get; set; } = null!;
    public decimal Temperature { get; set; }
    public DateTime SendTime { get; set; }
}