using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Response.Invoice;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class ProviderRepository : EFRepository<Provider>, IProviderRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public ProviderRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Provider> GetByUserID(long UserID)
        {
            return await _dbContext.Providers.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
        }

        public async Task<InvoiceResponse> GetProviderInvoice(long OrderID, int UserTypeID)
        {
            var orderInvoice = await (from o in _dbContext.Orders
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
                                      join t in _dbContext.Transactions on o.ID equals t.OrderID
                                      where o.ID == OrderID
                                      select new InvoiceResponse
                                      {
                                          OrderId = o.ID,
                                          ProviderName = put == null ? "" : $"{put.FirstName} {put.LastName}",
                                          ProviderAddress = put.Address,

                                          BuyerName = $"{u.FirstName} {u.LastName}",
                                          BuyerAddress = u.Address,

                                          DriverName = dut == null ? "" : $"{dut.FirstName} {dut.LastName}",
                                          DriverAddress = dut.Address,
                                          AmountPayable = _dbContext.OrderItems.Where(x => x.ItemID == o.ID).Sum(x => x.Quantity),
                                          PaymentMethod = ((TransactionType)t.TransactionType).ToString(),
                                          PaymentStatus = ((TransactionStatus)t.Status).ToString(),
                                          PaymentDate = t.TransactionTime,
                                          DeliveryCharges = 5.0m,
                                      }).FirstOrDefaultAsync();

            orderInvoice.OrderDetails  =await (from o in _dbContext.Orders
                                        join oi in _dbContext.OrderItems on o.ID equals oi.OrderID
                                        join it in _dbContext.Items on oi.ItemID equals it.ID
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
                                        where o.ID == OrderID
                                        select new OrderDetails
                                        {
                                            ProductID = oi.ItemID,
                                            ProductName = it.Name,
                                           Price =it.Price,
                                            Quantity=oi.Quantity, 
                                            PickUpFrom = put.Address,
                                            PickUpTime=dot.CreatedAt
                                            ,DeliveredTo =u.Username
                                            ,DeliveryTime=o.ActualDeliveryTime
                                            }).ToListAsync();

            return orderInvoice;
        }
    }
}
