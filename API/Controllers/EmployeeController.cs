using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> UpdateEmployee(string employeeNumber, EmployeeDto employeeDto)
        {
            if (employeeNumber != employeeDto.EmployeeNumber)
            {
                return BadRequest();
            }

            await _employeeService.UpdateEmployeeAsync(employeeNumber, employeeDto);
            return NoContent();
        }

        [HttpGet("pending-requests")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesWithPendingRequests()
        {
            var employees = await _employeeService.GetEmployeesWithPendingRequestsAsync();
            return Ok(employees);
        }
    }
}
