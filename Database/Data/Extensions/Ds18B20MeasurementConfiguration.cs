using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Extensions;

public class Ds18B20MeasurementConfiguration : IEntityTypeConfiguration<Ds18B20Measurement>
{
    public void Configure(EntityTypeBuilder<Ds18B20Measurement> builder)
    {
        builder.HasKey(bm => bm.Id);
        builder.HasOne(bm => bm.Sensor)
            .WithMany()
            .HasForeignKey(bm => bm.SensorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(bm => bm.Temperature).IsRequired().HasPrecision(18, 2);
        builder.Property(bm => bm.SendTime).IsRequired();
    }
}