namespace Database.Entities;

public class Bme280Measurement
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public Sensor Sensor { get; set; } = null!;
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal Pressure { get; set; }
    public DateTime SendTime { get; set; }
}