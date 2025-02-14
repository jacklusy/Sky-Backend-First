using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EmployeeManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, 
            ILogger<ApplicationDbContext>? logger = null)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
            _logger = logger ?? NullLogger<ApplicationDbContext>.Instance;
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

            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public async Task SeedDepartmentsAsync()
        {
            // Check if departments already exist
            if (await Departments.AnyAsync())
                return;

            var departments = new List<Department>
            {
                new Department { DepartmentName = "Engineering" },
                new Department { DepartmentName = "Sales" },
                new Department { DepartmentName = "Marketing" },
                new Department { DepartmentName = "Human Resources" },
                new Department { DepartmentName = "Finance" },
                new Department { DepartmentName = "Operations" },
                new Department { DepartmentName = "IT Support" },
                new Department { DepartmentName = "Product Management" },
                new Department { DepartmentName = "Quality Assurance" },
                new Department { DepartmentName = "Research & Development" }
            };

            await Departments.AddRangeAsync(departments);
            await SaveChangesAsync();
        }

        public async Task SeedPositionsAsync()
        {
            // Check if positions already exist
            if (await Positions.AnyAsync())
                return;

            var positions = new List<Position>
            {
                new Position { PositionName = "Technical Lead" },
                new Position { PositionName = "Senior Developer" },
                new Position { PositionName = "Software Engineer" },
                new Position { PositionName = "Project Manager" },
                new Position { PositionName = "Business Analyst" },
                new Position { PositionName = "QA Engineer" },
                new Position { PositionName = "DevOps Engineer" },
                new Position { PositionName = "UI/UX Designer" },
                new Position { PositionName = "Product Owner" },
                new Position { PositionName = "Scrum Master" }
            };

            await Positions.AddRangeAsync(positions);
            await SaveChangesAsync();
        }

        public async Task SeedEmployeesAsync()
        {
            // Check if employees already exist
            if (await Employees.AnyAsync())
                return;

            // Get all departments and positions
            var departments = await Departments.ToListAsync();
            var positions = await Positions.ToListAsync();

            // Debug logging
            _logger?.LogInformation("Available Departments: " + string.Join(", ", departments.Select(d => d.DepartmentName)));
            _logger?.LogInformation("Available Positions: " + string.Join(", ", positions.Select(p => p.PositionName)));

            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeNumber = "EMP001",
                    EmployeeName = "John Smith",
                    DepartmentId = departments.First(d => d.DepartmentName == "Engineering").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Technical Lead").PositionId,
                    GenderCode = "M",
                    VacationDaysLeft = 24,
                    Salary = 5000.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP002",
                    EmployeeName = "Jane Doe",
                    DepartmentId = departments.First(d => d.DepartmentName == "Engineering").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Senior Developer").PositionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4000.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP003",
                    EmployeeName = "Mike Johnson",
                    DepartmentId = departments.First(d => d.DepartmentName == "Engineering").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Project Manager").PositionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4500.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP004",
                    EmployeeName = "Sarah Wilson",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Business Analyst").PositionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4800.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP005",
                    EmployeeName = "David Brown",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "QA Engineer").PositionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 3800.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP006",
                    EmployeeName = "Emily Davis",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "DevOps Engineer").PositionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4200.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP007",
                    EmployeeName = "James Wilson",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Software Engineer").PositionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4100.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP008",
                    EmployeeName = "Lisa Anderson",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Senior Developer").PositionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4300.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP009",
                    EmployeeName = "Robert Taylor",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "UI/UX Designer").PositionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4700.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP010",
                    EmployeeName = "Emma Martinez",
                    DepartmentId = departments.First(d => d.DepartmentName == "Research & Development").DepartmentId,
                    PositionId = positions.First(p => p.PositionName == "Product Owner").PositionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4100.00m
                }
            };

            await Employees.AddRangeAsync(employees);
            await SaveChangesAsync();
        }
    }
} 