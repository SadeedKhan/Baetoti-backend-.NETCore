using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Response.Order;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IOrderItemRepository : IAsyncRepository<OrderItem>
    {
        Task<OrderResponse> GetAll();
        Task<OrderByIDResponse> GetByID(long ID);
    }
}
