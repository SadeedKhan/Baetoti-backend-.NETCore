using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Order;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class OrderRepository : EFRepository<Order>, IOrderRepository
    {
        private readonly BaetotiDbContext _dbContext;

        public OrderRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<OrderResponse> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
