using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Interfaces.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeService employeeService,
            ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{employeeNumber}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(string employeeNumber)
        {
            var employee = await _employeeService.GetEmployeeDetailsAsync(employeeNumber);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employeeDto)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { employeeNumber = createdEmployee.EmployeeNumber }, createdEmployee);
        }

        [HttpPut("{employeeNumber}")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateEmployee(string employeeNumber, [FromBody] EmployeeDto employeeDto)
        {
            await _employeeService.UpdateEmployeeAsync(employeeNumber, employeeDto);
            return NoContent();
        }

        [HttpGet("pending-requests")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesWithPendingRequests()
        {
            var employees = await _employeeService.GetEmployeesWithPendingRequestsAsync();
            return Ok(employees);
        }

        [HttpDelete("{employeeNumber}")]
        public async Task<ActionResult> DeleteEmployee(string employeeNumber)
        {
            await _employeeService.DeleteEmployeeAsync(employeeNumber);
            return NoContent();
        }
    }
}
