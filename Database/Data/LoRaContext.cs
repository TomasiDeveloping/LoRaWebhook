using Database.Data.Extensions;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Data;

public class LoRaContext : DbContext
{
    public LoRaContext(DbContextOptions<LoRaContext> options) : base(options)
    {
    }

    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorType> SensorTypes { get; set; }
    public DbSet<Bme280Measurement> Bme280Measurements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SensorConfiguration());

        modelBuilder.ApplyConfiguration(new SensorTypeConfiguration());

        modelBuilder.ApplyConfiguration(new Bme280MeasurementConfiguration());
    }
}