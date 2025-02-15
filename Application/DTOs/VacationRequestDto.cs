using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class VacationRequestDto
{
    [JsonIgnore]
    public int RequestId { get; set; }

    [Required]
    public string EmployeeNumber { get; set; }

    [JsonIgnore]
    public string? EmployeeName { get; set; }

    [Required]
    public string VacationType { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int TotalVacationDays { get; set; }

    [JsonIgnore]
    public string? RequestState { get; set; }

    [JsonIgnore]
    public string? ApprovedByEmployeeName { get; set; }

    [JsonIgnore]
    public DateTime? RequestSubmissionDate { get; set; }

    [JsonIgnore]
    public string? VacationDuration { get; set; }

    public string? Comments { get; set; }
}