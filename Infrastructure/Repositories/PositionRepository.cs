using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Persistence;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        public PositionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Position> GetByIdAsync(object id)
        {
            var positionId = Convert.ToInt32(id);
            return await _dbSet
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.PositionId == positionId);
        }

        public async Task<Position> GetByNameAsync(string positionName)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName == positionName);
        }
    }
} 