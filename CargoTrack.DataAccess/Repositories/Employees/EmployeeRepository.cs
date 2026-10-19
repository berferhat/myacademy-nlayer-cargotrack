using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Repositories.Employees
{
    public class EmployeeRepository(AppDbContext context) : GenericRepository<Employee>(context), IEmployeeRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Employee?> GetByAppUserIdAsync(Guid appUserId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.AppUserId == appUserId && !e.IsDeleted);
        }
    }
}
