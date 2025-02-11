using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // Primary key
        builder.HasKey(e => e.EmployeeNumber);

        // Property configurations
        builder.Property(e => e.EmployeeName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.GenderCode)
            .IsRequired()
            .HasMaxLength(1);

        // Relationships
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Position)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ReportedToEmployee)
            .WithMany(e => e.Subordinates)
            .HasForeignKey(e => e.ReportedToEmployeeNumber)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}