using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    [Required]
    [MaxLength(6)]
    public string EmployeeNumber { get; set; }

    [Required]
    [MaxLength(20)]
    public string EmployeeName { get; set; }

    public int DepartmentId { get; set; }
    public int PositionId { get; set; }

    [Required]
    [MaxLength(1)]
    public string GenderCode { get; set; }

    [MaxLength(6)]
    public string ReportedToEmployeeNumber { get; set; }

    [Range(0, 24, ErrorMessage = "Vacation days must be between 0 and 24")]
    public int VacationDaysLeft { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; }

    [ForeignKey("PositionId")]
    public virtual Position Position { get; set; }
    public virtual Employee ReportedToEmployee { get; set; }
    public virtual ICollection<Employee> Subordinates { get; set; }
    public virtual ICollection<VacationRequest> VacationRequests { get; set; }

    public Employee()
    {
        VacationDaysLeft = 24;
        Subordinates = new HashSet<Employee>();
        VacationRequests = new HashSet<VacationRequest>();
    }
}