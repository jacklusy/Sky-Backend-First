using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        /// <summary>
        /// Gets a department by ID including all its employees
        /// </summary>
        Task<Department> GetDepartmentWithEmployeesAsync(int departmentId);

        /// <summary>
        /// Gets all departments with their employee counts
        /// </summary>
        Task<IEnumerable<DepartmentWithCount>> GetDepartmentsWithEmployeeCountAsync();

        /// <summary>
        /// Checks if a department has any employees
        /// </summary>
        Task<bool> HasEmployeesAsync(int departmentId);

        /// <summary>
        /// Deletes a department after checking for employees
        /// </summary>
        Task DeleteDepartmentAsync(Department department);
    }
}
