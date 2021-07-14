using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Order;
using System.Threading.Tasks;
using System.Linq;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class OrderItemRepository : EFRepository<OrderItem>, IOrderItemRepository
    {
        private readonly BaetotiDbContext _dbContext;

        public OrderItemRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderResponse> GetAll()
        {
            var orderResponse = await (from c in _dbContext.Orders
                                       group c by c.Status into g
                                       select new OrderResponse
                                       {
                                           Completed = g.Count(c => c.Status == (int)OrderStatus.Completed),
                                           Pending = g.Count(c => c.Status == (int)OrderStatus.Pending),
                                           Approved = g.Count(c => c.Status == (int)OrderStatus.Approved),
                                           Rejected = g.Count(c => c.Status == (int)OrderStatus.Rejected),
                                           InProgress = g.Count(c => c.Status == (int)OrderStatus.InProgress),
                                           Ready = g.Count(c => c.Status == (int)OrderStatus.Ready),
                                           PickedUp = g.Count(c => c.Status == (int)OrderStatus.PickedUp),
                                           Delivered = g.Count(c => c.Status == (int)OrderStatus.Delivered),
                                           CancelledByCustomer = g.Count(c => c.Status == (int)OrderStatus.CancelledByCustomer),
                                           CancelledByProvider = g.Count(c => c.Status == (int)OrderStatus.CancelledByProvider),
                                           CancelledByDriver = g.Count(c => c.Status == (int)OrderStatus.CancelledByDriver),
                                           Complaints = g.Count(c => c.Status == (int)OrderStatus.Complaint)
                                       }).FirstOrDefaultAsync();
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

            var orderList = await (from o in _dbContext.Orders
                                   join u in _dbContext.Users on o.UserID equals u.ID
                                   join pr in _dbContext.ProviderOrders on o.ID equals pr.OrderID
                                   into providerOrderTemp
                                   from pot in providerOrderTemp.DefaultIfEmpty()
                                   join pu in _dbContext.Users on pot.ProviderID equals pu.ID
                                   into providerUserTemp
                                   from put in providerUserTemp.DefaultIfEmpty()
                                   join dor in _dbContext.DriverOrders on o.ID equals dor.OrderID
                                   into driverOrderTemp
                                   from dot in driverOrderTemp.DefaultIfEmpty()
                                   join du in _dbContext.Users on dot.DriverID equals du.ID
                                   into driverUserTemp
                                   from dut in driverUserTemp.DefaultIfEmpty()
                                   select new OrderStates
                                   {
                                       OrderID = o.ID,
                                       Buyer = $"{u.FirstName} {u.LastName}",
                                       Provider = put == null ? "" : $"{put.FirstName} {put.LastName}",
                                       Driver = dut == null ? "" : $"{dut.FirstName} {dut.LastName}",
                                       OrderAmount = _dbContext.OrderItems.Where(x => x.ItemID == o.ID).Sum(x => x.Quantity),
                                       PaymentType = "",
                                       Date = o.CreatedAt,
                                       ExpectedDeliveryTime = o.ActualDeliveryTime,
                                       DeliverOrPickup = "",
                                       OrderStatus = ((OrderStatus)o.Status).ToString()
                                   }).ToListAsync();
            orderResponse.OrderList = orderList;

            return orderResponse;
        }

        public async Task<OrderByIDResponse> GetByID(long ID)
        {
            return await (from o in _dbContext.Orders
                          join u in _dbContext.Users on o.UserID equals u.ID
                          join pr in _dbContext.ProviderOrders on o.ID equals pr.OrderID
                          into providerOrderTemp
                          from pot in providerOrderTemp.DefaultIfEmpty()
                          join pu in _dbContext.Users on pot.ProviderID equals pu.ID
                          into providerUserTemp
                          from put in providerUserTemp.DefaultIfEmpty()
                          join dor in _dbContext.DriverOrders on o.ID equals dor.OrderID
                          into driverOrderTemp
                          from dot in driverOrderTemp.DefaultIfEmpty()
                          join du in _dbContext.Users on dot.DriverID equals du.ID
                          into driverUserTemp
                          from dut in driverUserTemp.DefaultIfEmpty()
                          where o.ID == ID
                          select new OrderByIDResponse
                          {
                              orderDetail = new OrderDetail
                              {
                                  ActualDate = o.ActualDeliveryTime.ToShortDateString(),
                                  ActualTime = o.ActualDeliveryTime.ToShortTimeString(),
                                  ScheduleDate = o.ExpectedDeliveryTime.ToShortDateString(),
                                  ScheduleTime = o.ExpectedDeliveryTime.ToShortTimeString(),
                                  OrderReadyDate = o.OrderReadyTime.ToShortDateString(),
                                  OrderReadyTime = o.OrderReadyTime.ToShortTimeString()
                              },
                              customerDetail = new CustomerDetail
                              {
                                  Address = u.Address,
                                  City = u.City,
                                  Country = u.Country,
                                  Email = u.Email,
                                  Name = $"{u.FirstName} {u.LastName}",
                                  Phone = u.Phone,
                                  PostalCode = u.Zip
                              },
                              providerDetail = put == null ? new ProviderDetail() : new ProviderDetail
                              {
                                  Address = put.Address,
                                  City = put.City,
                                  Country = put.Country,
                                  Email = put.Email,
                                  Name = $"{put.FirstName} {put.LastName}",
                                  Phone = put.Phone,
                                  PostalCode = put.Zip
                              },
                              driverDetail = dut == null ? new DriverDetail() : new DriverDetail
                              {
                                  Address = dut.Address,
                                  City = dut.City,
                                  Country = dut.Country,
                                  Email = dut.Email,
                                  Name = $"{dut.FirstName} {dut.LastName}",
                                  Phone = dut.Phone,
                                  PostalCode = dut.Zip
                              },
                              paymentInfo = new PaymentInfo
                              {
                                  PaymnetMethod = "Online",
                                  PaymnetWindow = "01"
                              },
                              orderStatus = new OrderStatusResponse
                              {
                                  DeliverPickUp = "",
                                  OrderStatus = Convert.ToString((OrderStatus)o.Status)
                              }
                          }).FirstOrDefaultAsync();
        }
    }
}
