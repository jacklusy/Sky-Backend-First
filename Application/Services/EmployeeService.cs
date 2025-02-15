using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Infrastructure.Repositories;
using ValidationException = EmployeeManagement.Domain.Exceptions.ValidationException;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeDto> _validator;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IVacationRequestRepository _vacationRequestRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IValidator<EmployeeDto> validator,
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository,
            ILogger<EmployeeService> logger,
            IVacationRequestRepository vacationRequestRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _logger = logger;
            _vacationRequestRepository = vacationRequestRepository;
        }

        public async Task<EmployeeDto> GetEmployeeDetailsAsync(string employeeNumber)
        {
            var employee = await _employeeRepository.GetEmployeeWithDetailsAsync(employeeNumber);
            if (employee == null)
                throw new NotFoundException($"Employee with number {employeeNumber} not found.");

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            
            // Debug logging
            foreach (var employee in employees)
            {
                _logger.LogInformation($"Employee: {employee.EmployeeNumber}, " +
                    $"Department: {employee.Department?.DepartmentName ?? "null"}, " +
                    $"Position: {employee.Position?.PositionName ?? "null"}");
            }
            
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            
            // Debug logging for DTOs
            foreach (var dto in employeeDtos)
            {
                _logger.LogInformation($"DTO - Employee: {dto.EmployeeNumber}, " +
                    $"Department: {dto.DepartmentName ?? "null"}, " +
                    $"Position: {dto.PositionName ?? "null"}");
            }
            
            return employeeDtos;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            try
            {
                // Validate the DTO
                var validationResult = await _validator.ValidateAsync(employeeDto);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for employee: {EmployeeNumber}. Errors: {Errors}", 
                        employeeDto.EmployeeNumber,
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                        
                    throw new ValidationException(validationResult.Errors);
                }

                // Check if employee number already exists
                var existingEmployee = await _employeeRepository.GetByIdAsync(employeeDto.EmployeeNumber);
                if (existingEmployee != null)
                    throw new BusinessException($"Employee with number {employeeDto.EmployeeNumber} already exists.");

                // Get Department and Position
                var department = await _departmentRepository.GetByNameAsync(employeeDto.DepartmentName);
                var position = await _positionRepository.GetByNameAsync(employeeDto.PositionName);

                // Log available departments and positions for debugging
                _logger.LogInformation("Available Departments: {Departments}", 
                    string.Join(", ", (await _departmentRepository.GetAllAsync()).Select(d => d.DepartmentName)));
                _logger.LogInformation("Available Positions: {Positions}", 
                    string.Join(", ", (await _positionRepository.GetAllAsync()).Select(p => p.PositionName)));

                if (department == null)
                    throw new NotFoundException($"Department '{employeeDto.DepartmentName}' not found.");
                if (position == null)
                    throw new NotFoundException($"Position '{employeeDto.PositionName}' not found.");

                // Map DTO to entity
                var employee = _mapper.Map<Employee>(employeeDto);
                
                // Ensure gender code is single character
                employee.GenderCode = employee.GenderCode?.Substring(0, 1);

                // Set Department and Position IDs
                employee.DepartmentId = department.DepartmentId;
                employee.PositionId = position.PositionId;

                // Handle ReportedToEmployee if provided
                if (!string.IsNullOrEmpty(employeeDto.ReportedToEmployeeName))
                {
                    var employees = await _employeeRepository.GetAllAsync();
                    var reportedToEmployee = employees.FirstOrDefault(e => e.EmployeeName == employeeDto.ReportedToEmployeeName);
                    
                    if (reportedToEmployee == null)
                        throw new NotFoundException($"Reported-to employee '{employeeDto.ReportedToEmployeeName}' not found.");
                        
                    employee.ReportedToEmployeeNumber = reportedToEmployee.EmployeeNumber;
                }

                // Set default values if not provided
                if (employee.VacationDaysLeft == 0)
                    employee.VacationDaysLeft = 24;

                _logger.LogInformation(
                    "Creating employee: Number={EmployeeNumber}, Name={Name}, Department={Department}({DepartmentId}), Position={Position}({PositionId}), Gender={Gender}",
                    employee.EmployeeNumber,
                    employee.EmployeeName,
                    employeeDto.DepartmentName,
                    employee.DepartmentId,
                    employeeDto.PositionName,
                    employee.PositionId,
                    employee.GenderCode
                );

                await _employeeRepository.AddAsync(employee);
                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee: {EmployeeNumber}. Error: {Error}", 
                    employeeDto.EmployeeNumber, 
                    ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task UpdateEmployeeAsync(string employeeNumber, EmployeeDto employeeDto)
        {
            var validationResult = await _validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var employee = await _employeeRepository.GetByIdAsync(employeeNumber);
            if (employee == null)
                throw new NotFoundException($"Employee with number {employeeNumber} not found.");

            _mapper.Map(employeeDto, employee);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesWithPendingRequestsAsync()
        {
            var employees = await _employeeRepository.GetEmployeesWithPendingRequestsAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task DeleteEmployeeAsync(string employeeNumber)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeNumber);
            if (employee == null)
                throw new NotFoundException($"Employee with number {employeeNumber} not found.");

            // Check if employee has any active vacation requests
            var hasActiveRequests = await _vacationRequestRepository.HasActiveRequestsAsync(employeeNumber);
            if (hasActiveRequests)
                throw new BusinessException("Cannot delete employee with active vacation requests.");

            try
            {
                await _employeeRepository.DeleteAsync(employee);
                _logger.LogInformation("Employee {EmployeeNumber} deleted successfully", employeeNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {EmployeeNumber}", employeeNumber);
                throw new BusinessException("Error occurred while deleting the employee", ex);
            }
        }
    }
}