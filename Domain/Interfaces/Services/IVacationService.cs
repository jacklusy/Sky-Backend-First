using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Domain.Interfaces.Services
{
    public interface IVacationService
    {
        Task<VacationRequestDto> SubmitVacationRequestAsync(VacationRequestDto requestDto);
        Task<VacationRequestDto> ApproveVacationRequestAsync(int requestId, string approverEmployeeNumber);
        Task<VacationRequestDto> DeclineVacationRequestAsync(int requestId, string declinerEmployeeNumber);
        Task<IEnumerable<VacationRequestDto>> GetPendingApprovalsAsync(string approverEmployeeNumber);
        Task<IEnumerable<VacationRequestDto>> GetEmployeeVacationHistoryAsync(string employeeNumber);
        Task<VacationRequestDto> GetRequestAsync(int requestId);
    }
}