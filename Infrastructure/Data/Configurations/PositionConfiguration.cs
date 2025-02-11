using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        // Primary key
        builder.HasKey(p => p.PositionId);

        // Property configurations
        builder.Property(p => p.PositionName)
            .IsRequired()
            .HasMaxLength(30);
    }
}