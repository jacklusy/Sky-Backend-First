using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class VacationRequest
{
    public int RequestId { get; set; }

    [Required]
    public DateTime RequestSubmissionDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    [Required]
    [MaxLength(6)]
    public string EmployeeNumber { get; set; }

    [Required]
    [MaxLength(1)]
    public string VacationTypeCode { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }

    [Required]
    public int TotalVacationDays { get; set; }

    [Required]
    public int RequestStateId { get; set; }

    [MaxLength(6)]
    public string ApprovedByEmployeeNumber { get; set; }

    [MaxLength(6)]
    public string DeclinedByEmployeeNumber { get; set; }

    public virtual Employee Employee { get; set; }
    public virtual VacationType VacationType { get; set; }
    public virtual RequestState RequestState { get; set; }
    public virtual Employee ApprovedByEmployee { get; set; }
    public virtual Employee DeclinedByEmployee { get; set; }
}