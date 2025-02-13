using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Interfaces.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
            IDepartmentService departmentService,
            ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{departmentId}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int departmentId)
        {
            var department = await _departmentService.GetDepartmentWithEmployeesAsync(departmentId);
            return Ok(department);
        }

        [HttpGet("with-employee-count")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsWithEmployeeCount()
        {
            var departments = await _departmentService.GetDepartmentsWithEmployeeCountAsync();
            return Ok(departments);
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto departmentDto)
        {
            var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentDto);
            return CreatedAtAction(
                nameof(GetDepartment), 
                new { departmentId = createdDepartment.DepartmentId }, 
                createdDepartment);
        }

        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, DepartmentDto departmentDto)
        {
            if (departmentId != departmentDto.DepartmentId)
            {
                return BadRequest();
            }

            await _departmentService.UpdateDepartmentAsync(departmentId, departmentDto);
            return NoContent();
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            await _departmentService.DeleteDepartmentAsync(departmentId);
            return NoContent();
        }
    }
}