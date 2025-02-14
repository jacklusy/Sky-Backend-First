# Employee Management System

A robust ASP.NET Core Web API for managing employees, departments, positions, and vacation requests. This system provides comprehensive functionality for employee management and vacation tracking.

## Features

- **Employee Management**

  - Create, read, update employee information
  - Track employee details including department, position, and reporting structure
  - Manage employee vacation balances

- **Department Management**

  - Organize employees by departments
  - Track department hierarchies
  - View department statistics

- **Position Management**

  - Define and manage job positions
  - Associate employees with positions
  - Track position-specific data

- **Vacation Management**
  - Submit vacation requests
  - Approve/decline vacation requests
  - Track vacation history
  - Support multiple vacation types (Annual, Sick, Unpaid, Day Off, Business Trip)
  - Automatic vacation balance management

## Technology Stack

- **Framework**: .NET 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **API Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Object Mapping**: AutoMapper
- **Architecture**: Clean Architecture with Domain-Driven Design principles

## Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or preferred IDE

## Getting Started

1. **Clone the Repository**

   ```bash
   git clone [repository-url]
   cd EmployeeManagement
   ```

2. **Update Database Connection**

   - Open `appsettings.json`
   - Update the `DefaultConnection` string to point to your SQL Server instance

3. **Apply Database Migrations**

   ```bash
   dotnet ef database update
   ```

4. **Run the Application**

   ```bash
   dotnet run
   ```

5. **Access Swagger Documentation**
   - Navigate to `https://localhost:56653/swagger` in your browser
   - Explore available endpoints and test API functionality

## Project Structure

EmployeeManagement/
├── API/
│ ├── Controllers/
│ └── Middleware/
├── Application/
│ ├── DTOs/
│ ├── Mappings/
│ ├── Services/
│ └── Validators/
├── Domain/
│ ├── Entities/
│ ├── Exceptions/
│ ├── Interfaces/
│ └── Enums/
└── Infrastructure/
├── Persistence/
├── Repositories/
└── Data/

## Database Schema

The system includes the following main entities:

- Employees
- Departments
- Positions
- VacationRequests
- VacationTypes
- RequestStates

## API Endpoints

### Employees

- `GET /api/Employee` - Get all employees
- `GET /api/Employee/{employeeNumber}` - Get employee details
- `POST /api/Employee` - Create new employee
- `PUT /api/Employee/{employeeNumber}` - Update employee

### Vacation Requests

- `POST /api/VacationRequest` - Submit vacation request
- `GET /api/VacationRequest/{requestId}` - Get request details
- `POST /api/VacationRequest/{requestId}/approve` - Approve request
- `POST /api/VacationRequest/{requestId}/decline` - Decline request

### Departments

- `GET /api/Department` - Get all departments
- `GET /api/Department/{departmentId}` - Get department details
- `GET /api/Department/with-employee-count` - Get departments with employee counts

## Data Seeding

The system includes seed data for:

- Common departments
- Standard positions
- Vacation types
- Request states

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
