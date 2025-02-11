using System.ComponentModel.DataAnnotations;

public class Department
{
    public int DepartmentId { get; set; }

    [Required]
    [MaxLength(50)]
    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee>? Employees { get; set; }
}