using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Response.Order;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IOrderRepository : IAsyncRepository<OrderItem>
    {
        Task<OrderResponse> GetAll();
    }
}
