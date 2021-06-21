using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IUserRoleRepository : IAsyncRepository<UserRoles>
    {
        Task<UserRoles> GetByUserId(int UserId);
    }
}
