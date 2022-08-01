using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Data.Extensions;

public class SensorTypeConfiguration : IEntityTypeConfiguration<SensorType>
{
    public void Configure(EntityTypeBuilder<SensorType> builder)
    {
        builder.HasKey(st => st.Id);
        builder.Property(st => st.Name).IsRequired();
        builder.Property(st => st.CreatedAt).IsRequired();
        builder.Property(st => st.UpdatedAt).IsRequired(false);
    }
}