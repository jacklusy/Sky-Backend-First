using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Primary key
        builder.HasKey(d => d.DepartmentId);

        // Property configurations
        builder.Property(d => d.DepartmentName)
            .IsRequired()
            .HasMaxLength(50);
    }
}