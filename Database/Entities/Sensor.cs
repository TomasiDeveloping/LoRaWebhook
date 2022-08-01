namespace Database.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string DevAddress { get; set; } = string.Empty;
    public SensorType SensorType { get; set; } = null!;
    public Guid SensorTypeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}