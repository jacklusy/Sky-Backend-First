using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class VacationRequestRepository : GenericRepository<VacationRequest>, IVacationRequestRepository
    {
        public VacationRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VacationRequest>> GetEmployeeVacationHistoryAsync(string employeeNumber)
        {
            return await _context.VacationRequests
                .Include(vr => vr.VacationType)
                .Include(vr => vr.RequestState)
                .Include(vr => vr.ApprovedByEmployee)
                .Include(vr => vr.DeclinedByEmployee)
                .Where(vr => vr.EmployeeNumber == employeeNumber)
                .OrderByDescending(vr => vr.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VacationRequest>> GetPendingApprovalsAsync(string approverEmployeeNumber)
        {
            return await _context.VacationRequests
                .Include(vr => vr.Employee)
                .Include(vr => vr.VacationType)
                .Include(vr => vr.RequestState)
                .Where(vr => vr.Employee.ReportedToEmployeeNumber == approverEmployeeNumber)
                .Where(vr => vr.RequestStateId == (int)RequestStateEnum.Submitted)
                .OrderBy(vr => vr.RequestSubmissionDate)
                .ToListAsync();
        }

        public async Task<VacationRequest> GetRequestWithDetailsAsync(int requestId)
        {
            return await _context.VacationRequests
                .Include(vr => vr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(vr => vr.VacationType)
                .Include(vr => vr.RequestState)
                .Include(vr => vr.ApprovedByEmployee)
                .Include(vr => vr.DeclinedByEmployee)
                .FirstOrDefaultAsync(vr => vr.RequestId == requestId);
        }

        public async Task<IEnumerable<VacationRequest>> GetRequestsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.VacationRequests
                .Include(vr => vr.Employee)
                .Include(vr => vr.VacationType)
                .Include(vr => vr.RequestState)
                .Where(vr => vr.StartDate <= endDate && vr.EndDate >= startDate)
                .OrderBy(vr => vr.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VacationRequest>> GetRequestsByDepartmentAsync(int departmentId)
        {
            return await _context.VacationRequests
                .Include(vr => vr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(vr => vr.VacationType)
                .Include(vr => vr.RequestState)
                .Where(vr => vr.Employee.DepartmentId == departmentId)
                .OrderByDescending(vr => vr.RequestSubmissionDate)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetVacationStatisticsAsync(int year)
        {
            var requests = await _context.VacationRequests
                .Include(vr => vr.VacationType)
                .Where(vr => vr.StartDate.Year == year)
                .Where(vr => vr.RequestStateId == (int)RequestStateEnum.Approved)
                .ToListAsync();

            return new Dictionary<string, int>
            {
                ["TotalRequests"] = requests.Count,
                ["TotalDays"] = requests.Sum(r => r.TotalVacationDays),
                ["SickLeave"] = requests.Count(r => r.VacationTypeCode == "S"),
                ["AnnualLeave"] = requests.Count(r => r.VacationTypeCode == "A"),
                ["UnpaidLeave"] = requests.Count(r => r.VacationTypeCode == "U"),
                ["BusinessTrips"] = requests.Count(r => r.VacationTypeCode == "B"),
                ["DaysOff"] = requests.Count(r => r.VacationTypeCode == "O")
            };
        }

        public async Task<bool> HasActiveRequestsAsync(string employeeNumber)
        {
            return await _context.VacationRequests
                .AnyAsync(vr => vr.EmployeeNumber == employeeNumber && 
                               vr.RequestStateId == (int)RequestStateEnum.Submitted);
        }
    }
}