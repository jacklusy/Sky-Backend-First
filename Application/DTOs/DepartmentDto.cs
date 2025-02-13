namespace EmployeeManagement.Application.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}