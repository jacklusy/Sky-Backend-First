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

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeDto> _validator;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IValidator<EmployeeDto> validator,
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
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
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var validationResult = await _validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get Department and Position IDs
            var department = await _departmentRepository.GetByNameAsync(employeeDto.DepartmentName);
            var position = await _positionRepository.GetByNameAsync(employeeDto.PositionName);

            if (department == null)
                throw new NotFoundException($"Department '{employeeDto.DepartmentName}' not found.");
            if (position == null)
                throw new NotFoundException($"Position '{employeeDto.PositionName}' not found.");

            var employee = _mapper.Map<Employee>(employeeDto);
            employee.DepartmentId = department.DepartmentId;
            employee.PositionId = position.PositionId;

            await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(employee);
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
    }
}