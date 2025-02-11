using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class VacationTypeConfiguration : IEntityTypeConfiguration<VacationType>
{
    public void Configure(EntityTypeBuilder<VacationType> builder)
    {
        // Primary key
        builder.HasKey(v => v.VacationTypeCode);

        // Property configurations
        builder.Property(v => v.VacationTypeName)
            .IsRequired()
            .HasMaxLength(20);
    }
}