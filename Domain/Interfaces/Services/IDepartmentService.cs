using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Domain.Interfaces.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Gets all departments
        /// </summary>
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();

        /// <summary>
        /// Gets a department by its ID including its employees
        /// </summary>
        Task<DepartmentDto> GetDepartmentWithEmployeesAsync(int departmentId);

        /// <summary>
        /// Gets all departments with their employee counts
        /// </summary>
        Task<IEnumerable<DepartmentDto>> GetDepartmentsWithEmployeeCountAsync();

        /// <summary>
        /// Creates a new department
        /// </summary>
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto);

        /// <summary>
        /// Updates an existing department
        /// </summary>
        Task UpdateDepartmentAsync(int departmentId, DepartmentDto departmentDto);

        /// <summary>
        /// Deletes a department
        /// </summary>
        Task DeleteDepartmentAsync(int departmentId);
    }
} 