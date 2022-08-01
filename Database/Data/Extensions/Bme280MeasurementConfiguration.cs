using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Extensions;

public class Bme280MeasurementConfiguration : IEntityTypeConfiguration<Bme280Measurement>
{
    public void Configure(EntityTypeBuilder<Bme280Measurement> builder)
    {
        builder.HasKey(bm => bm.Id);
        builder.HasOne(bm => bm.Sensor)
            .WithMany()
            .HasForeignKey(bm => bm.SensorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(bm => bm.Humidity).IsRequired().HasPrecision(18, 2);
        builder.Property(bm => bm.Pressure).IsRequired().HasPrecision(18, 2);
        builder.Property(bm => bm.Temperature).IsRequired().HasPrecision(18, 2);
        builder.Property(bm => bm.SendTime).IsRequired();
    }
}