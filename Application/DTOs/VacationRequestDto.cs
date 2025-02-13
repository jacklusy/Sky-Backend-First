public class VacationRequestDto
{
    public int RequestId { get; set; }
    public string EmployeeNumber { get; set; }
    public string EmployeeName { get; set; }
    public string VacationType { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalVacationDays { get; set; }
    public string RequestState { get; set; }
    public string ApprovedByEmployeeName { get; set; }
    public DateTime RequestSubmissionDate { get; set; }
    public string VacationDuration { get; set; }
}