public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeDetailsAsync(string employeeNumber);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
    Task UpdateEmployeeAsync(string employeeNumber, EmployeeDto employeeDto);
    Task<IEnumerable<EmployeeDto>> GetEmployeesWithPendingRequestsAsync();
    Task DeleteEmployeeAsync(string employeeNumber);
}