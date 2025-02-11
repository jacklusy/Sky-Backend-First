using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class VacationRequestConfiguration : IEntityTypeConfiguration<VacationRequest>
{
    public void Configure(EntityTypeBuilder<VacationRequest> builder)
    {
        // Primary key
        builder.HasKey(v => v.RequestId);

        // Property configurations
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(v => v.Employee)
            .WithMany(e => e.VacationRequests)
            .HasForeignKey(v => v.EmployeeNumber)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.VacationType)
            .WithMany(vt => vt.VacationRequests)
            .HasForeignKey(v => v.VacationTypeCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.RequestState)
            .WithMany(rs => rs.VacationRequests)
            .HasForeignKey(v => v.RequestStateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.ApprovedByEmployee)
            .WithMany()
            .HasForeignKey(v => v.ApprovedByEmployeeNumber)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.DeclinedByEmployee)
            .WithMany()
            .HasForeignKey(v => v.DeclinedByEmployeeNumber)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}