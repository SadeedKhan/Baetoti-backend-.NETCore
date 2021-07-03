using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IDriverRepository : IAsyncRepository<Driver>
    {
        Task<Driver> GetByUserID(long UserID);
    }
}
