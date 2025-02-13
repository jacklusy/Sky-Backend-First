using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories
{
    public interface IVacationRequestRepository : IGenericRepository<VacationRequest>
    {
        /// <summary>
        /// Gets the vacation history for a specific employee
        /// </summary>
        /// <param name="employeeNumber">The employee number to get history for</param>
        /// <returns>Collection of approved vacation requests for the employee</returns>
        Task<IEnumerable<VacationRequest>> GetEmployeeVacationHistoryAsync(string employeeNumber);

        /// <summary>
        /// Gets all pending vacation requests that need approval from a specific approver
        /// </summary>
        /// <param name="approverEmployeeNumber">The employee number of the approver</param>
        /// <returns>Collection of pending vacation requests</returns>
        Task<IEnumerable<VacationRequest>> GetPendingApprovalsAsync(string approverEmployeeNumber);

        /// <summary>
        /// Gets a vacation request with all related entities included
        /// </summary>
        /// <param name="requestId">The ID of the vacation request</param>
        /// <returns>The vacation request with all navigation properties loaded</returns>
        Task<VacationRequest> GetRequestWithDetailsAsync(int requestId);

        /// <summary>
        /// Gets all vacation requests for a specific time period
        /// </summary>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Collection of vacation requests within the specified period</returns>
        Task<IEnumerable<VacationRequest>> GetRequestsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all vacation requests for a specific department
        /// </summary>
        /// <param name="departmentId">The department ID</param>
        /// <returns>Collection of vacation requests for the specified department</returns>
        Task<IEnumerable<VacationRequest>> GetRequestsByDepartmentAsync(int departmentId);

        /// <summary>
        /// Gets statistics about vacation requests for reporting purposes
        /// </summary>
        /// <param name="year">The year to get statistics for</param>
        /// <returns>Dictionary containing various statistics about vacation requests</returns>
        Task<Dictionary<string, int>> GetVacationStatisticsAsync(int year);
    }
}