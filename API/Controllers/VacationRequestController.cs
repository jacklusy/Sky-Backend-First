using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Interfaces.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacationRequestController : ControllerBase
    {
        private readonly IVacationService _vacationService;

        public VacationRequestController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpPost]
        public async Task<ActionResult<VacationRequestDto>> SubmitRequest(VacationRequestDto requestDto)
        {
            var result = await _vacationService.SubmitVacationRequestAsync(requestDto);
            return CreatedAtAction(nameof(GetRequest), new { requestId = result.RequestId }, result);
        }

        [HttpGet("{requestId}")]
        public async Task<ActionResult<VacationRequestDto>> GetRequest(int requestId)
        {
            var request = await _vacationService.GetRequestAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPost("{requestId}/approve")]
        public async Task<ActionResult<VacationRequestDto>> ApproveRequest(int requestId, string approverEmployeeNumber)
        {
            var result = await _vacationService.ApproveVacationRequestAsync(requestId, approverEmployeeNumber);
            return Ok(result);
        }

        [HttpPost("{requestId}/decline")]
        public async Task<ActionResult<VacationRequestDto>> DeclineRequest(int requestId, string declinerEmployeeNumber)
        {
            var result = await _vacationService.DeclineVacationRequestAsync(requestId, declinerEmployeeNumber);
            return Ok(result);
        }

        [HttpGet("pending/{approverEmployeeNumber}")]
        public async Task<ActionResult<IEnumerable<VacationRequestDto>>> GetPendingApprovals(string approverEmployeeNumber)
        {
            var requests = await _vacationService.GetPendingApprovalsAsync(approverEmployeeNumber);
            return Ok(requests);
        }

        [HttpGet("history/{employeeNumber}")]
        public async Task<ActionResult<IEnumerable<VacationRequestDto>>> GetEmployeeHistory(string employeeNumber)
        {
            var history = await _vacationService.GetEmployeeVacationHistoryAsync(employeeNumber);
            return Ok(history);
        }
    }
}