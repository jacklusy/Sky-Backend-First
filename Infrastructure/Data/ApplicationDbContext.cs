using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<VacationType> VacationTypes { get; set; }
    public DbSet<RequestState> RequestStates { get; set; }
    public DbSet<VacationRequest> VacationRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new VacationTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RequestStateConfiguration());
        modelBuilder.ApplyConfiguration(new VacationRequestConfiguration());

        // Seed initial data
        SeedInitialData(modelBuilder);
    }

    private void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Seed RequestStates
        modelBuilder.Entity<RequestState>().HasData(
            new RequestState { StateId = 1, StateName = "Submitted" },
            new RequestState { StateId = 2, StateName = "Approved" },
            new RequestState { StateId = 3, StateName = "Declined" }
        );

        // Seed VacationTypes
        modelBuilder.Entity<VacationType>().HasData(
            new VacationType { VacationTypeCode = "S", VacationTypeName = "Sick" },
            new VacationType { VacationTypeCode = "U", VacationTypeName = "Unpaid" },
            new VacationType { VacationTypeCode = "A", VacationTypeName = "Annual" },
            new VacationType { VacationTypeCode = "O", VacationTypeName = "Day Off" },
            new VacationType { VacationTypeCode = "B", VacationTypeName = "Business Trip" }
        );
    }
}