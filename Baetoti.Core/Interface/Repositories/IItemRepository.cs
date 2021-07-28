using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Request.Item;
using Baetoti.Shared.Response.Item;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IItemRepository : IAsyncRepository<Item>
    {
        Task<ItemResponse> GetFilteredItemsDataAsync(FilterRequest filterRequest);

        Task<ItemResponse> GetAll();

        Task<ItemResponseByID> GetByID(long ItemID);
    }
}
