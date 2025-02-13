using AutoMapper;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Application.DTOs;
using System;

namespace EmployeeManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.PositionName,
                    opt => opt.MapFrom(src => src.Position.PositionName))
                .ForMember(dest => dest.ReportedToEmployeeName,
                    opt => opt.MapFrom(src => src.ReportedToEmployee.EmployeeName))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.GenderCode == "M" ? "Male" : "Female"));

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

            // Add reverse mapping if needed
            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<VacationRequestDto, VacationRequest>().ReverseMap();
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