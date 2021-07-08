using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Response.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public UserRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<UserResponse> GetFilteredUsersDataAsync(FilterRequest filterRequest)
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
    }
}
