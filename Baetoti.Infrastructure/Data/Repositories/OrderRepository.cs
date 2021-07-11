using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Order;
using System.Threading.Tasks;
using System.Linq;
using Baetoti.Shared.Enum;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class OrderRepository : EFRepository<OrderItem>, IOrderRepository
    {
        private readonly BaetotiDbContext _dbContext;

        public OrderRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderResponse> GetAll()
        {
            var cartList = (from c in _dbContext.Carts select c);

            var orderResponse = new OrderResponse();
            orderResponse.Completed = cartList.Count(c => c.Status == (int)OrderStatus.Completed);
            orderResponse.Pending = cartList.Count(c => c.Status == (int)OrderStatus.Pending);
            orderResponse.Approved = cartList.Count(c => c.Status == (int)OrderStatus.Approved);
            orderResponse.Rejected = cartList.Count(c => c.Status == (int)OrderStatus.Rejected);
            orderResponse.InProgress = cartList.Count(c => c.Status == (int)OrderStatus.InProgress);
            orderResponse.Ready = cartList.Count(c => c.Status == (int)OrderStatus.Ready);
            orderResponse.PickedUp = cartList.Count(c => c.Status == (int)OrderStatus.PickedUp);
            orderResponse.Delivered = cartList.Count(c => c.Status == (int)OrderStatus.Delivered);
            orderResponse.CancelledByCustomer = cartList.Count(c => c.Status == (int)OrderStatus.CancelledByCustomer);
            orderResponse.CancelledByProvider = cartList.Count(c => c.Status == (int)OrderStatus.CancelledByProvider);
            orderResponse.CancelledByDriver = cartList.Count(c => c.Status == (int)OrderStatus.CancelledByDriver);
            orderResponse.Complaints = cartList.Count(c => c.Status == (int)OrderStatus.Complaint);
            orderResponse.AverageRating = 0;
            orderResponse.RevenueGain = 0;
            orderResponse.RevenueLoss = 0;
            orderResponse.AverageCheckValue = 0;
            orderResponse.FeedBackCollected = 0;
            orderResponse.OrderWithFullAdoptionAndCompliance = 0;
            orderResponse.OrdersDeliverUnder30Minutes = 0;
            orderResponse.AdoptionPercentage = 0;
            orderResponse.CompliancePercentage = 0;
            orderResponse.OrderRunRate = 0;

            var providerOrderStates = new ProviderOrderStates();
            var providerOrder = from po in _dbContext.ProviderOrders select po;
            providerOrderStates.Approved = providerOrder.Count(p => p.Status == (int)ProviderOrderStatus.Approved);
            providerOrderStates.Rejected = providerOrder.Count(p => p.Status == (int)ProviderOrderStatus.Rejected);
            providerOrderStates.Canceled = providerOrder.Count(p => p.Status == (int)ProviderOrderStatus.Cancelled);
            orderResponse.ProviderOrder = providerOrderStates;

            var driverOrderStates = new DriverOrderStates();
            var driverOrder = from d in _dbContext.DriverOrders select d;
            driverOrderStates.Pending = driverOrder.Count(d => d.Status == (int)DriverOrderStatus.Pending);
            driverOrderStates.Delivered = driverOrder.Count(d => d.Status == (int)DriverOrderStatus.Delivered);
            driverOrderStates.PickedUp = driverOrder.Count(d => d.Status == (int)DriverOrderStatus.PickedUp);
            orderResponse.DriverOrder = driverOrderStates;

            var orderStates = new OrderStates();

            //orderResponse.OrderList = orderStates;

            return orderResponse;
        }
    }
}
