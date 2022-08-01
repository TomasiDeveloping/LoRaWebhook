using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Extensions;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.DevAddress).IsUnique();
        builder.HasOne(s => s.SensorType)
            .WithMany()
            .HasForeignKey(s => s.SensorTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(s => s.DevAddress).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired(false);
    }
}