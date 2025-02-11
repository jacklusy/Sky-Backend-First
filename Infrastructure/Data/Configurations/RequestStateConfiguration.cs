using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class RequestStateConfiguration : IEntityTypeConfiguration<RequestState>
{
    public void Configure(EntityTypeBuilder<RequestState> builder)
    {
        // Primary key
        builder.HasKey(r => r.StateId);

        // Property configurations
        builder.Property(r => r.StateName)
            .IsRequired()
            .HasMaxLength(10);
    }
}