namespace Core.DataTransferObjects;

public class SensorDto
{
    public Guid Id { get; set; }
    public string DevAddress { get; set; } = null!;
    public string? SensorTypeName { get; set; }
    public Guid SensorTypeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}