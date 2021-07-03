using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IProviderRepository : IAsyncRepository<Provider>
    {
        Task<Provider> GetByUserID(long UserID);
    }
}
