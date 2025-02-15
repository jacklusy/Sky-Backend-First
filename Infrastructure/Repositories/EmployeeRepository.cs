using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Employee> GetByIdAsync(object id)
    {
        var employeeNumber = id.ToString();
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.ReportedToEmployee)
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
    }

    public override async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.ReportedToEmployee)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Employee> GetEmployeeWithDetailsAsync(string employeeNumber)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.ReportedToEmployee)
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
    }

    public async Task<bool> HasOverlappingVacationsAsync(string employeeNumber, DateTime startDate, DateTime endDate)
    {
        return await _context.VacationRequests
            .AnyAsync(v => v.EmployeeNumber == employeeNumber &&
                          v.RequestStateId == (int)RequestStateEnum.Approved &&
                          ((v.StartDate <= startDate && v.EndDate >= startDate) ||
            (v.StartDate <= endDate && v.EndDate >= endDate) ||
                           (v.StartDate >= startDate && v.EndDate <= endDate)));
    }

    public async Task UpdateVacationBalanceAsync(string employeeNumber, int daysToDeduct)
    {
        var employee = await _dbSet.FindAsync(employeeNumber);
        if (employee != null)
        {
            employee.VacationDaysLeft -= daysToDeduct;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithPendingRequestsAsync()
    {
        return await _dbSet
            .Include(e => e.VacationRequests)
            .Where(e => e.VacationRequests.Any(v => v.RequestStateId == (int)RequestStateEnum.Submitted))
            .ToListAsync();
    }

    public override async Task DeleteAsync(object id)
    {
        var employeeNumber = id.ToString();
        
        // First, delete all vacation requests
        var vacationRequests = await _context.VacationRequests
            .Where(v => v.EmployeeNumber == employeeNumber)
            .ToListAsync();
        
        if (vacationRequests.Any())
        {
            _context.VacationRequests.RemoveRange(vacationRequests);
            await _context.SaveChangesAsync();
        }

        // Then find and delete the employee
        var employee = await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.ReportedToEmployee)
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

        if (employee != null)
        {
            try
            {
                // Remove any employees that report to this employee
                var reportingEmployees = await _dbSet
                    .Where(e => e.ReportedToEmployeeNumber == employeeNumber)
                    .ToListAsync();

                foreach (var reportingEmployee in reportingEmployees)
                {
                    reportingEmployee.ReportedToEmployeeNumber = null;
                }
                await _context.SaveChangesAsync();

                // Now remove the employee
                _dbSet.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                throw new Exception($"Failed to delete employee {employeeNumber}. Error: {ex.Message}", ex);
            }
        }
    }
}