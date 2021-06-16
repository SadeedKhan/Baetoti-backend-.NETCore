using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> AuthenticateUser(User user);

        Task<List<string>> GetRolesAsync(User user);

    }
}
