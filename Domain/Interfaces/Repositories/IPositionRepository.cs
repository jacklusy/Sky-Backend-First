using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        Task<Position> GetByNameAsync(string positionName);
    }
} 