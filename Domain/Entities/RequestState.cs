using System.ComponentModel.DataAnnotations;

public class RequestState
{
    public int StateId { get; set; }

    [Required]
    [MaxLength(10)]
    public string StateName { get; set; }

    public virtual ICollection<VacationRequest> VacationRequests { get; set; }
}