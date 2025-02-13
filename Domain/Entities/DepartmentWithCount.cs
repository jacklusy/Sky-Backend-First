using System.Collections.Generic;

namespace EmployeeManagement.Domain.Entities
{
    public class DepartmentWithCount
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
} 