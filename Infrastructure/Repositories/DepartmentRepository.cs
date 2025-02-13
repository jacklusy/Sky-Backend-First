using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces.Repositories;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Department> GetDepartmentWithEmployeesAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<DepartmentWithCount>> GetDepartmentsWithEmployeeCountAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Select(d => new DepartmentWithCount
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    EmployeeCount = d.Employees.Count,
                    Employees = d.Employees
                })
                .ToListAsync();
        }

        public async Task<bool> HasEmployeesAsync(int departmentId)
        {
            return await _context.Employees
                .AnyAsync(e => e.DepartmentId == departmentId);
        }

        public async Task DeleteDepartmentAsync(Department department)
        {
            var hasEmployees = await HasEmployeesAsync(department.DepartmentId);
            if (hasEmployees)
            {
                throw new BusinessException($"Cannot delete department {department.DepartmentName} because it has employees.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
