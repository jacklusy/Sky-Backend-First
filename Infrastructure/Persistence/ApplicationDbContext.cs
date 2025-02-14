using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EmployeeManagement.Infrastructure.Persistence
{
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

            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public async Task SeedDepartmentsAsync()
        {
            // Check if departments already exist
            if (await Departments.AnyAsync())
                return;

            var departments = new List<Department>();
            var departmentNames = new[]
            {
                "Sales", "Marketing", "Engineering", "Research & Development",
                "Human Resources", "Finance", "Operations", "Customer Service",
                "Legal", "IT Support", "Quality Assurance", "Product Management",
                "Business Development", "Public Relations", "Administration",
                "Supply Chain", "Training", "Security", "Facilities", "Strategy"
            };

            for (int i = 0; i < departmentNames.Length; i++)
            {
                departments.Add(new Department
                {
                    DepartmentName = departmentNames[i]
                });
            }

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
                new Position { PositionName = "Software Engineer" },
                new Position { PositionName = "Senior Developer" },
                new Position { PositionName = "Project Manager" },
                new Position { PositionName = "Business Analyst" },
                new Position { PositionName = "Quality Assurance Engineer" },
                new Position { PositionName = "DevOps Engineer" },
                new Position { PositionName = "System Administrator" },
                new Position { PositionName = "Database Administrator" },
                new Position { PositionName = "UI/UX Designer" },
                new Position { PositionName = "Product Owner" },
                new Position { PositionName = "Scrum Master" },
                new Position { PositionName = "Technical Lead" },
                new Position { PositionName = "Solution Architect" },
                new Position { PositionName = "Data Scientist" },
                new Position { PositionName = "Frontend Developer" },
                new Position { PositionName = "Backend Developer" },
                new Position { PositionName = "Full Stack Developer" },
                new Position { PositionName = "Mobile Developer" },
                new Position { PositionName = "Cloud Engineer" },
                new Position { PositionName = "Security Engineer" }
            };

            await Positions.AddRangeAsync(positions);
            await SaveChangesAsync();
        }

        public async Task SeedEmployeesAsync()
        {
            // Check if employees already exist
            if (await Employees.AnyAsync())
                return;

            // Get the actual department IDs from the database
            var departmentIds = await Departments.Select(d => d.DepartmentId).ToListAsync();
            if (!departmentIds.Any())
            {
                // If no departments exist, seed them first
                await SeedDepartmentsAsync();
                departmentIds = await Departments.Select(d => d.DepartmentId).ToListAsync();
            }

            // Get position IDs
            var positionIds = await Positions.Select(p => p.PositionId).ToListAsync();
            if (!positionIds.Any())
            {
                await SeedPositionsAsync();
                positionIds = await Positions.Select(p => p.PositionId).ToListAsync();
            }

            // Safely get department and position IDs
            var departmentId = departmentIds.FirstOrDefault();
            var positionId = positionIds.FirstOrDefault();

            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeNumber = "EMP001",
                    EmployeeName = "John Smith",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "M",
                    VacationDaysLeft = 24,
                    Salary = 5000.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP002",
                    EmployeeName = "Jane Doe",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4000.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP003",
                    EmployeeName = "Mike Johnson",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4500.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP004",
                    EmployeeName = "Sarah Wilson",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4800.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP005",
                    EmployeeName = "David Brown",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 3800.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP006",
                    EmployeeName = "Emily Davis",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4200.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP007",
                    EmployeeName = "James Wilson",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4100.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP008",
                    EmployeeName = "Lisa Anderson",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "F",
                    ReportedToEmployeeNumber = "EMP004",
                    VacationDaysLeft = 24,
                    Salary = 4300.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP009",
                    EmployeeName = "Robert Taylor",
                    DepartmentId = departmentId,
                    PositionId = positionId,
                    GenderCode = "M",
                    ReportedToEmployeeNumber = "EMP001",
                    VacationDaysLeft = 24,
                    Salary = 4700.00m
                },
                new Employee
                {
                    EmployeeNumber = "EMP010",
                    EmployeeName = "Emma Martinez",
                    DepartmentId = departmentId,
                    PositionId = positionId,
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