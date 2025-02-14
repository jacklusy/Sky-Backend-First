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
}