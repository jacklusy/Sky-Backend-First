using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Domain.Interfaces.Services;
using EmployeeManagement.Application.DTOs;
using ValidationException = EmployeeManagement.Domain.Exceptions.ValidationException;

namespace EmployeeManagement.Application.Services
{
    public class VacationService : IVacationService
    {
        private readonly IVacationRequestRepository _vacationRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<VacationRequestDto> _validator;

        public VacationService(
            IVacationRequestRepository vacationRequestRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IValidator<VacationRequestDto> validator)
        {
            _vacationRequestRepository = vacationRequestRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<VacationRequestDto> SubmitVacationRequestAsync(VacationRequestDto requestDto)
        {
            var validationResult = await _validator.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Check if employee exists
            var employee = await _employeeRepository.GetByIdAsync(requestDto.EmployeeNumber);
            if (employee == null)
                throw new NotFoundException($"Employee with number {requestDto.EmployeeNumber} not found.");

            var hasOverlap = await _employeeRepository.HasOverlappingVacationsAsync(
                requestDto.EmployeeNumber,
                requestDto.StartDate,
                requestDto.EndDate);

            if (hasOverlap)
                throw new BusinessException("Vacation dates overlap with existing requests.");

            var request = _mapper.Map<VacationRequest>(requestDto);
            request.RequestStateId = (int)RequestStateEnum.Submitted;
            request.RequestSubmissionDate = DateTime.UtcNow;

            await _vacationRequestRepository.AddAsync(request);
            return _mapper.Map<VacationRequestDto>(request);
        }

        public async Task<VacationRequestDto> ApproveVacationRequestAsync(int requestId, string approverEmployeeNumber)
        {
            var request = await _vacationRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new NotFoundException($"Vacation request with ID {requestId} not found.");

            request.RequestStateId = (int)RequestStateEnum.Approved;
            request.ApprovedByEmployeeNumber = approverEmployeeNumber;

            await _vacationRequestRepository.UpdateAsync(request);
            await _employeeRepository.UpdateVacationBalanceAsync(request.EmployeeNumber, request.TotalVacationDays);

            return _mapper.Map<VacationRequestDto>(request);
        }

        public async Task<VacationRequestDto> DeclineVacationRequestAsync(int requestId, string declinerEmployeeNumber)
        {
            var request = await _vacationRequestRepository.GetByIdAsync(requestId);
            if (request == null)
                throw new NotFoundException($"Vacation request with ID {requestId} not found.");

            request.RequestStateId = (int)RequestStateEnum.Declined;
            request.DeclinedByEmployeeNumber = declinerEmployeeNumber;

            await _vacationRequestRepository.UpdateAsync(request);
            return _mapper.Map<VacationRequestDto>(request);
        }

        public async Task<IEnumerable<VacationRequestDto>> GetPendingApprovalsAsync(string approverEmployeeNumber)
        {
            var requests = await _vacationRequestRepository.GetPendingApprovalsAsync(approverEmployeeNumber);
            return _mapper.Map<IEnumerable<VacationRequestDto>>(requests);
        }

        public async Task<IEnumerable<VacationRequestDto>> GetEmployeeVacationHistoryAsync(string employeeNumber)
        {
            var history = await _vacationRequestRepository.GetEmployeeVacationHistoryAsync(employeeNumber);
            return _mapper.Map<IEnumerable<VacationRequestDto>>(history);
        }

        public async Task<VacationRequestDto> GetRequestAsync(int requestId)
        {
            var request = await _vacationRequestRepository.GetRequestWithDetailsAsync(requestId);
            if (request == null)
                throw new NotFoundException($"Vacation request with ID {requestId} not found.");

            return _mapper.Map<VacationRequestDto>(request);
        }
    }
} 