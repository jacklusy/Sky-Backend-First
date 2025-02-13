using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        // Seed Departments
        var departments = new List<Department>();
        for (int i = 1; i <= 20; i++)
        {
            departments.Add(new Department 
            { 
                DepartmentId = i,
                DepartmentName = $"Department {i}"
            });
        }
        modelBuilder.Entity<Department>().HasData(departments);

        // Seed Positions
        var positions = new List<Position>();
        for (int i = 1; i <= 20; i++)
        {
            positions.Add(new Position 
            { 
                PositionId = i,
                PositionName = $"Position {i}"
            });
        }
        modelBuilder.Entity<Position>().HasData(positions);

        // Seed Employees
        var employees = new List<Employee>();
        for (int i = 1; i <= 10; i++)
        {
            employees.Add(new Employee 
            { 
                EmployeeNumber = $"EMP{i:D3}",
                EmployeeName = $"Employee {i}",
                DepartmentId = (i % 20) + 1,
                PositionId = (i % 20) + 1,
                GenderCode = i % 2 == 0 ? "M" : "F",
                ReportedToEmployeeNumber = i > 1 ? $"EMP{(i-1):D3}" : null,
                Salary = 2500 + (i * 100)
            });
        }
        modelBuilder.Entity<Employee>().HasData(employees);
    }
}