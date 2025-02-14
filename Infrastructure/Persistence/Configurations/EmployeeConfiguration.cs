using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeNumber);
            
            builder.Property(e => e.EmployeeNumber)
                .HasMaxLength(6);

            builder.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.GenderCode)
                .IsRequired()
                .HasMaxLength(1);  // This is fine, but we need to handle the input

            // ... other configurations
        }
    }
} 