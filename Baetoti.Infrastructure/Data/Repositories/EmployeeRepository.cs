using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : EFRepository<Employee>, IEmployeeRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public EmployeeRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<List<string>> IEmployeeRepository.GetRolesAsync(Employee user)
        {
            return await (from ur in _dbContext.EmployeeRoles
                          join
                          r in _dbContext.Roles on ur.RoleId equals r.ID
                          where ur.EmployeeId == user.ID
                          select r.Name).ToListAsync();
        }

        async Task<Employee> IEmployeeRepository.AuthenticateUser(Employee user)
        {
            return await _dbContext.Employee.Where(x => x.Username.ToLower() == user.Username.ToLower() && x.EmployeeStatus == (int)EmployementStatus.Active).FirstOrDefaultAsync();
        }

    }
}
