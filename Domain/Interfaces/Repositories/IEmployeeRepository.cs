using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;

namespace EmployeeManagement.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee> GetEmployeeWithDetailsAsync(string employeeNumber);
        Task<IEnumerable<Employee>> GetEmployeesWithPendingRequestsAsync();
        Task<bool> HasOverlappingVacationsAsync(string employeeNumber, DateTime startDate, DateTime endDate);
        Task UpdateVacationBalanceAsync(string employeeNumber, int daysToDeduct);
    }
}