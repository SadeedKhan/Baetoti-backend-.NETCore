using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Response.Invoice;
using Baetoti.Shared.Response.Item;
using Baetoti.Shared.Response.User;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {

        private readonly BaetotiDbContext _dbContext;
        private readonly IConfiguration _config;

        public UserRepository(BaetotiDbContext dbContext, IConfiguration config) : base(dbContext)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<User> GetByMobileNumberAsync(string mobileNumber)
        {
            return await _dbContext.Users.Where(x => x.Phone == mobileNumber).FirstOrDefaultAsync();
        }

        public async Task<OnBoardingResponse> GetOnBoardingDataAsync()
        {
            // Provider
            var providers = from p in _dbContext.Providers
                            join u in _dbContext.Users on p.UserID equals u.ID
                            select new
                            {
                                p.UserID,
                                Name = $"{u.FirstName} {u.LastName}",
                                u.Email,
                                u.Phone,
                                u.Address,
                                RequestedDate = p.CreatedAt,
                                p.ProviderStatus
                            };
            var provider = new ProviderOnBoardingRequest();
            var providerState = new UserStates();
            providerState.Pending = providers.Where(x => x.ProviderStatus == 2).Count();
            providerState.Approved = providers.Where(x => x.ProviderStatus == 3).Count();
            providerState.Rejected = providers.Where(x => x.ProviderStatus == 4).Count();
            provider.userStates = providerState;
            provider.userList = await providers.Where(x => x.ProviderStatus == 2).Select(p => new OnBoardingUserList
            {
                UserID = p.UserID,
                Name = p.Name,
                Email = p.Email,
                MobileNumber = p.Phone,
                Address = p.Address,
                RequestDate = p.RequestedDate
            }).ToListAsync();

            // Driver
            var drivers = from d in _dbContext.Drivers
                          join u in _dbContext.Users on d.UserID equals u.ID
                          select new
                          {
                              d.UserID,
                              Name = $"{u.FirstName} {u.LastName}",
                              u.Email,
                              u.Phone,
                              u.Address,
                              RequestedDate = d.CreatedAt,
                              d.DriverStatus
                          };
            var driver = new DriverOnBoardingRequest();
            var driverState = new UserStates();
            driverState.Pending = drivers.Where(x => x.DriverStatus == 2).Count();
            driverState.Approved = drivers.Where(x => x.DriverStatus == 3).Count();
            driverState.Rejected = drivers.Where(x => x.DriverStatus == 4).Count();
            driver.userStates = driverState;
            driver.userList = await drivers.Where(x => x.DriverStatus == 2).Select(p => new OnBoardingUserList
            {
                UserID = p.UserID,
                Name = p.Name,
                Email = p.Email,
                MobileNumber = p.Phone,
                Address = p.Address,
                RequestDate = p.RequestedDate
            }).ToListAsync();

            // Combined
            var combined = new ProviderAndDriverOnBoardingRequest();
            var combinedState = new UserStates();
            combinedState.Pending = providerState.Pending + driverState.Pending;
            combinedState.Approved = providerState.Approved + driverState.Approved;
            combinedState.Rejected = providerState.Rejected + driverState.Rejected;
            combined.userStates = combinedState;
            combined.userList = new List<OnBoardingUserList>();
            if (provider.userList.Count > 0)
                combined.userList.AddRange(provider.userList);
            if (driver.userList.Count > 0)
                combined.userList.AddRange(driver.userList);

            // Final Result
            var onBoardingResponse = new OnBoardingResponse();
            onBoardingResponse.providers = provider;
            onBoardingResponse.drivers = driver;
            onBoardingResponse.providerAndDrivers = combined;

            return onBoardingResponse;
        }

        public async Task<UserResponse> GetAllUsersDataAsync()
        {
            // Users
            var userList = from u in _dbContext.Users
                           join p in _dbContext.Providers on u.ID equals p.UserID
                           into userProvider
                           from p in userProvider.DefaultIfEmpty()
                           join d in _dbContext.Drivers on u.ID equals d.UserID
                           into userProviderDriver
                           from d in userProviderDriver.DefaultIfEmpty()
                           select new UserList
                           {
                               UserID = u.ID,
                               Name = $"{u.FirstName} {u.LastName}",
                               Revenue = "0",
                               MobileNumber = u.Phone,
                               Address = u.Address,
                               SignUpDate = u.CreatedAt,
                               UserStatus = u.UserStatus == 1 ? "Active" : "Inactive",
                               ProviderStatus = p == null ? "-" : p.ProviderStatus == 2 ? "Pending" : p.ProviderStatus == 3 ? "Approved" : "Rejected",
                               DriverStatus = d == null ? "-" : d.DriverStatus == 2 ? "Pending" : d.DriverStatus == 3 ? "Approved" : "Rejected"
                           };
            var userSammary = new UserSummary();
            userSammary.TotalUser = userList.Count();
            userSammary.ActiveUser = userList.Where(x => x.UserStatus == "Active").Count();
            userSammary.NewUser = userList.Where(x => x.SignUpDate >= DateTime.Now.AddMonths(-1)).Count();
            userSammary.LiveUser = 0;
            userSammary.ReportedUser = 0;

            // Providers
            var providerSummary = new ProviderSummary();
            var providers = from u in _dbContext.Users
                            join p in _dbContext.Providers on u.ID equals p.UserID
                            select new
                            {
                                u.UserStatus,
                                p.CreatedAt
                            };
            providerSummary.TotalProvider = providers.Count();
            providerSummary.NewProvider = providers.Count(x => x.CreatedAt >= DateTime.Now.AddMonths(-1));
            providerSummary.ActiveProvider = providers.Count(x => x.UserStatus == 1);
            providerSummary.LiveProvider = 0;
            providerSummary.ReportedProvider = 0;

            // Drivers
            var driverSummary = new DriverSummary();
            var drivers = from u in _dbContext.Users
                          join d in _dbContext.Drivers on u.ID equals d.UserID
                          select new
                          {
                              u.UserStatus,
                              d.CreatedAt
                          };
            driverSummary.TotalDriver = drivers.Count();
            driverSummary.NewDriver = drivers.Count(x => x.CreatedAt >= DateTime.Now.AddMonths(-1));
            driverSummary.ActiveDriver = drivers.Count(x => x.UserStatus == 1);
            driverSummary.LiveDriver = 0;
            driverSummary.ReportedDriver = 0;

            // Over All
            var userResponse = new UserResponse();
            userResponse.userList = await userList.ToListAsync();
            userResponse.userSummary = userSammary;
            userResponse.providerSummary = providerSummary;
            userResponse.driverSummary = driverSummary;

            return userResponse;
        }

        public async Task<UserResponse> GetFilteredUsersDataAsync(UserFilterRequest filterRequest)
        {
            // Users
            var userList = from u in _dbContext.Users
                           join p in _dbContext.Providers on u.ID equals p.UserID
                           into userProvider
                           from p in userProvider.DefaultIfEmpty()
                           join d in _dbContext.Drivers on u.ID equals d.UserID
                           into userProviderDriver
                           from d in userProviderDriver.DefaultIfEmpty()
                           select new UserList
                           {
                               UserID = u.ID,
                               Name = $"{u.FirstName} {u.LastName}",
                               Revenue = "0",
                               MobileNumber = u.Phone,
                               Address = u.Address,
                               SignUpDate = u.CreatedAt,
                               UserStatus = u.UserStatus == 1 ? "Active" : "Inactive",
                               ProviderStatus = p == null ? "-" : p.ProviderStatus == 2 ? "Pending" : p.ProviderStatus == 3 ? "Approved" : "Rejected",
                               DriverStatus = d == null ? "-" : d.DriverStatus == 2 ? "Pending" : d.DriverStatus == 3 ? "Approved" : "Rejected"
                           };
            var userSammary = new UserSummary();
            userSammary.TotalUser = userList.Count();
            userSammary.ActiveUser = userList.Where(x => x.UserStatus == "Active").Count();
            userSammary.NewUser = userList.Where(x => x.SignUpDate >= DateTime.Now.AddMonths(-1)).Count();
            userSammary.LiveUser = 0;

            // Providers
            var providerSummary = new ProviderSummary();
            var providers = from u in _dbContext.Users
                            join p in _dbContext.Providers on u.ID equals p.UserID
                            select new
                            {
                                u.UserStatus,
                                p.CreatedAt
                            };
            providerSummary.TotalProvider = providers.Count();
            providerSummary.NewProvider = providers.Count(x => x.CreatedAt >= DateTime.Now.AddMonths(-1));
            providerSummary.ActiveProvider = providers.Count(x => x.UserStatus == 1);
            providerSummary.LiveProvider = 0;
            providerSummary.ReportedProvider = 0;

            // Drivers
            var driverSummary = new DriverSummary();
            var drivers = from u in _dbContext.Users
                          join d in _dbContext.Drivers on u.ID equals d.UserID
                          select new
                          {
                              u.UserStatus,
                              d.CreatedAt
                          };
            driverSummary.TotalDriver = drivers.Count();
            driverSummary.NewDriver = drivers.Count(x => x.CreatedAt >= DateTime.Now.AddMonths(-1));
            driverSummary.ActiveDriver = drivers.Count(x => x.UserStatus == 1);
            driverSummary.LiveDriver = 0;

            // Over All
            var userResponse = new UserResponse();
            userResponse.userList = await userList.ToListAsync();
            userResponse.userSummary = userSammary;
            userResponse.providerSummary = providerSummary;
            userResponse.driverSummary = driverSummary;

            return userResponse;
        }

        public async Task<InvoiceResponse> GetBuyerInvoice(long OrderID, int UserTypeID)
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

            orderInvoice.OrderDetails = await (from o in _dbContext.Orders
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
                                                   Price = it.Price,
                                                   Quantity = oi.Quantity,
                                                   PickUpFrom = put.Address,
                                                   PickUpTime = dot.CreatedAt
                                                   ,
                                                   DeliveredTo = u.Username
                                                   ,
                                                   DeliveryTime = o.ActualDeliveryTime
                                               }).ToListAsync();

            return orderInvoice;
        }

        public async Task<UserProfile> GetUserProfile(long UserID)
        {
            var userProfile = new UserProfile();
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString("Default")))
            {
                var param = new DynamicParameters();
                param.Add("@UserID", UserID);
                using (var m = db.QueryMultiple("[baetoti].[GetUserProfile]", param, commandType: CommandType.StoredProcedure))
                {

                    var buyer = m.ReadFirstOrDefault<BuyerResponse>();
                    userProfile.buyer = buyer;

                    var buyerHistory = m.Read<BuyerHistory>().ToList();
                    userProfile.buyer.buyerHistory = buyerHistory;

                    var provider = m.ReadFirstOrDefault<ProviderResponse>();
                    userProfile.provider = provider;

                    var storeSchedule = m.Read<WeekDays>().ToList();
                    userProfile.provider.weekDays = storeSchedule;

                    var items = m.Read<ItemListResponse>().ToList();
                    userProfile.provider.Items = items;

                    var order = m.Read<ProviderOrders>().ToList();
                    userProfile.provider.Orders = order;

                    var order2 = m.Read<ProviderOrders2>().ToList();
                    userProfile.provider.Orders2 = order2;

                    var driver = m.ReadFirstOrDefault<DriverResponse>();
                    userProfile.driver = driver;

                    var deliveryDetail = m.Read<DeliveryDetail>().ToList();
                    userProfile.driver.deliveryDetails = deliveryDetail;

                    var analyticalData = m.ReadFirstOrDefault<AnalyticalData>();
                    userProfile.analytics.analyticalData = analyticalData;

                    var cancelledOrder = m.Read<UserCancelledOrder>().ToList();
                    var userCancelledOrder = new UserCancelledOrder2();
                    cancelledOrder.ForEach(x =>
                    {
                        userCancelledOrder.CancelledOrder.Add(x.CancelledOrder);
                        userCancelledOrder.TotalOrder.Add(x.TotalOrder);
                        userCancelledOrder.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.userCancelledOrder = userCancelledOrder;

                    var providerOrderPrice = m.Read<OrderPrice2>().ToList();
                    var orderPrice = new OrderPrice();
                    providerOrderPrice.ForEach(x =>
                    {
                        orderPrice.TotalOrder.Add(x.TotalOrder);
                        orderPrice.TotalPrice.Add(x.TotalPrice);
                        orderPrice.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.provider.orderPrice = orderPrice;

                    var providerCancelledOrder = m.Read<CancelledOrder>().ToList();
                    var providerCancelledOrder2 = new CancelledOrder2();
                    providerCancelledOrder.ForEach(x =>
                    {
                        providerCancelledOrder2.Cancelled.Add(x.Cancelled);
                        providerCancelledOrder2.TotalOrder.Add(x.TotalOrder);
                        providerCancelledOrder2.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.provider.cancelledOrder = providerCancelledOrder2;

                    var driverDeliveryTimeAccuracy = m.Read<DeliveryTimeAccuracy2>().ToList();
                    var deliveryTimeAccuracy = new DeliveryTimeAccuracy();
                    driverDeliveryTimeAccuracy.ForEach(x =>
                    {
                        deliveryTimeAccuracy.Schedule.Add(x.Schedule);
                        deliveryTimeAccuracy.Actual.Add(x.Actual);
                        deliveryTimeAccuracy.Date.Add(x.Date);
                    });
                    userProfile.analytics.driver.deliveryTimeAccuracy = deliveryTimeAccuracy;

                    var driverCancelledOrder2 = m.Read<DriverCancelledOrder2>().ToList();
                    var driverCancelledOrder = new DriverCancelledOrder();
                    driverCancelledOrder2.ForEach(x =>
                    {
                        driverCancelledOrder.TotalOrder.Add(x.TotalOrder);
                        driverCancelledOrder.Cancelled.Add(x.Cancelled);
                        driverCancelledOrder.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.driver.driverCancelledOrder = driverCancelledOrder;

                    var totalAcceptedOrder2 = m.Read<TotalAcceptedOrder2>().ToList();
                    var totalAcceptedOrder = new TotalAcceptedOrder();
                    totalAcceptedOrder2.ForEach(x =>
                    {
                        totalAcceptedOrder.TotalOrder.Add(x.TotalOrder);
                        totalAcceptedOrder.AcceptedOrder.Add(x.AcceptedOrder);
                        totalAcceptedOrder.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.order.totalAcceptedOrder = totalAcceptedOrder;

                    var averageOrderPrice2 = m.Read<AverageOrderPrice2>().ToList();
                    var averageOrderPrice = new AverageOrderPrice();
                    averageOrderPrice2.ForEach(x =>
                    {
                        averageOrderPrice.TotalOrder.Add(x.TotalOrder);
                        averageOrderPrice.OrderDate.Add(x.OrderDate);
                    });
                    userProfile.analytics.order.averageOrderPrice = averageOrderPrice;

                    var orderTimeAccuracy2 = m.Read<OrderTimeAccuracy2>().ToList();
                    var orderTimeAccuracy = new OrderTimeAccuracy();
                    orderTimeAccuracy2.ForEach(x =>
                    {
                        orderTimeAccuracy.Schedule.Add(x.Schedule);
                        orderTimeAccuracy.Actual.Add(x.Actual);
                        orderTimeAccuracy.Date.Add(x.Date);
                    });
                    userProfile.analytics.order.orderTimeAccuracy = orderTimeAccuracy;

                    var transactionsHistory = m.Read<TransactionsHistory>().ToList();
                    userProfile.wallet.transactionsHistory = transactionsHistory;
                }
            }
            return userProfile;
        }
    }
}
