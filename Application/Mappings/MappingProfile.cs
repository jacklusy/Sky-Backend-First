using AutoMapper;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Application.DTOs;
using System;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmployeeManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : null))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.PositionName : null))
                .ForMember(dest => dest.ReportedToEmployeeName, opt => opt.MapFrom(src => src.ReportedToEmployee != null ? src.ReportedToEmployee.EmployeeName : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCode));

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.GenderCode, opt => opt.MapFrom(src => src.Gender.Substring(0, 1)))
                .ForMember(dest => dest.DepartmentId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PositionId,
                    opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCode));

            CreateMap<VacationRequest, VacationRequestDto>()
                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src => src.Employee.EmployeeName))
                .ForMember(dest => dest.VacationType,
                    opt => opt.MapFrom(src => src.VacationType.VacationTypeName))
                .ForMember(dest => dest.RequestState,
                    opt => opt.MapFrom(src => src.RequestState.StateName))
                .ForMember(dest => dest.ApprovedByEmployeeName,
                    opt => opt.MapFrom(src => src.ApprovedByEmployee.EmployeeName))
                .ForMember(dest => dest.VacationDuration,
                    opt => opt.MapFrom(src => CalculateVacationDuration(src.StartDate, src.EndDate)));

            // Add the reverse mapping
            CreateMap<VacationRequestDto, VacationRequest>()
                .ForMember(dest => dest.VacationTypeCode, 
                    opt => opt.MapFrom(src => src.VacationType))
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.VacationType, opt => opt.Ignore())
                .ForMember(dest => dest.RequestState, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedByEmployee, opt => opt.Ignore())
                .ForMember(dest => dest.DeclinedByEmployee, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedByEmployeeNumber, opt => opt.Ignore())
                .ForMember(dest => dest.DeclinedByEmployeeNumber, opt => opt.Ignore());

            // Add reverse mapping if needed
            CreateMap<Department, DepartmentDto>().ReverseMap();
        }

        private string CalculateVacationDuration(DateTime startDate, DateTime endDate)
        {
            var days = (endDate - startDate).Days + 1;
            return days >= 7
                ? $"{days / 7} weeks {days % 7} days"
                : $"{days} days";
        }
    }
}