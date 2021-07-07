using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Response.Employee;
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

        Task<List<EmployeeResponse>> IEmployeeRepository.GetAll(long Id)
        {
            return (from e in _dbContext.Employee
                    join er in _dbContext.EmployeeRoles
                    on e.ID equals er.EmployeeId
                    join r in _dbContext.Roles on er.RoleId equals r.ID
                    where Id==0 || Id==e.ID && e.MarkAsDeleted == false
                    select new EmployeeResponse
                    {
                        ID = e.ID,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JoiningDate = e.JoiningDate,
                        Location = e.Location,
                        DepartmentID = e.DepartmentID,
                        DesignationID = e.DesignationID,
                        Username = e.Username,
                        Gender = e.Gender,
                        Shift = e.Shift,
                        RoleId = er.RoleId,
                        Role = r.Name,
                        Email = e.Email,
                        DOB = e.DOB,
                        Phone = e.Phone,
                        ReportTo = e.ReportTo,
                        Address = e.Address,
                        Goals = e.Goals,
                        Skills = e.Skills,
                        RefreshToken = e.RefreshToken,
                        EmployeeStatus = e.EmployeeStatus,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy
                    }).ToListAsync();
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
