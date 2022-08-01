namespace Database.Entities
{
    public class SensorType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
