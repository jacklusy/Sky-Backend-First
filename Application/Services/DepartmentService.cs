using AutoMapper;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Domain.Interfaces.Services;
using ValidationException = EmployeeManagement.Domain.Exceptions.ValidationException;

namespace EmployeeManagement.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<DepartmentDto> _validator;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IValidator<DepartmentDto> validator)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> GetDepartmentWithEmployeesAsync(int departmentId)
        {
            var department = await _departmentRepository.GetDepartmentWithEmployeesAsync(departmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID {departmentId} not found.");

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsWithEmployeeCountAsync()
        {
            var departments = await _departmentRepository.GetDepartmentsWithEmployeeCountAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            var validationResult = await _validator.ValidateAsync(departmentDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var department = _mapper.Map<Department>(departmentDto);
            await _departmentRepository.AddAsync(department);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task UpdateDepartmentAsync(int departmentId, DepartmentDto departmentDto)
        {
            var validationResult = await _validator.ValidateAsync(departmentDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID {departmentId} not found.");

            _mapper.Map(departmentDto, department);
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new NotFoundException($"Department with ID {departmentId} not found.");

            await _departmentRepository.DeleteDepartmentAsync(department);
        }
    }
} 