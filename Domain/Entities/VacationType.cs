using System.ComponentModel.DataAnnotations;

public class VacationType
{
    [Key]
    [MaxLength(1)]
    public string VacationTypeCode { get; set; }

    [Required]
    [MaxLength(20)]
    public string VacationTypeName { get; set; }

    public virtual ICollection<VacationRequest> VacationRequests { get; set; }
}