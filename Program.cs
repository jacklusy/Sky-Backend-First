using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Reflection;
using EmployeeManagement.API.Middleware;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Domain.Interfaces.Services;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.Validators;
using Swashbuckle.AspNetCore.Swagger;
using EmployeeManagement.Application.Mappings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Explicitly load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// After loading configuration
var allConnectionStrings = builder.Configuration.GetSection("ConnectionStrings").GetChildren();
foreach (var connection in allConnectionStrings)
{
    Console.WriteLine($"Found connection string: {connection.Key} = {connection.Value}");
}

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1"
    });
});

// Add DbContext configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string 'DefaultConnection' not found.");
}

// Register ApplicationDbContext with logging
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
    
    // Get ILoggerFactory from the service provider
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
    
    // Configure the context to use the logger
    options.UseLoggerFactory(loggerFactory);
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

// Register Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IVacationRequestRepository, VacationRequestRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();

// Register Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IVacationService, VacationService>();

// Register Validators
builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<DepartmentDto>, DepartmentValidator>();
builder.Services.AddScoped<IValidator<VacationRequestDto>, VacationRequestValidator>();
builder.Services.AddScoped<IValidator<PositionDto>, PositionValidator>();

var app = builder.Build();

// Initialize Database
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Delete the existing database
        await context.Database.EnsureDeletedAsync();
        
        // Apply migrations to create a new database
        await context.Database.MigrateAsync();
        
        // Seed data
        await context.SeedDepartmentsAsync();
        await context.SeedPositionsAsync();
        await context.SeedEmployeesAsync();
        
        // Log seeded data
        var departments = await context.Departments.ToListAsync();
        var positions = await context.Positions.ToListAsync();
        
        Console.WriteLine("\nSeeded Departments:");
        foreach (var dept in departments)
        {
            Console.WriteLine($"- {dept.DepartmentName} (ID: {dept.DepartmentId})");
        }
        
        Console.WriteLine("\nSeeded Positions:");
        foreach (var pos in positions)
        {
            Console.WriteLine($"- {pos.PositionName} (ID: {pos.PositionId})");
        }

        var employees = await context.Employees.ToListAsync();
        Console.WriteLine("\nSeeded Employees:");
        foreach (var emp in employees)
        {
            Console.WriteLine($"- {emp.EmployeeName} (Number: {emp.EmployeeNumber})");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
    }
    throw;
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
    });
}

app.UseHttpsRedirection();

// Add custom exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();