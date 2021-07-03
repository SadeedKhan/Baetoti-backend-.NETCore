using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.User;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OnBoardingResponse> GetonBoardingDataAsync()
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
            provider.userList = providers.Where(x => x.ProviderStatus == 2).Select(p => new OnBoardingUserList
            {
                UserID = p.UserID,
                Name = p.Name,
                Email = p.Email,
                MobileNumber = p.Phone,
                Address = p.Address,
                RequestDate = p.RequestedDate
            }).ToList();

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
            driver.userList = drivers.Where(x => x.DriverStatus == 2).Select(p => new OnBoardingUserList
            {
                UserID = p.UserID,
                Name = p.Name,
                Email = p.Email,
                MobileNumber = p.Phone,
                Address = p.Address,
                RequestDate = p.RequestedDate
            }).ToList();

            // Combined
            var combined = new ProviderAndDriverOnBoardingRequest();
            var combinedState = new UserStates();
            combinedState.Pending = providerState.Pending + driverState.Pending;
            combinedState.Approved = providerState.Approved + driverState.Approved;
            combinedState.Rejected = providerState.Rejected + driverState.Rejected;
            combined.userStates = combinedState;
            combined.userList.AddRange(provider.userList);
            combined.userList.AddRange(driver.userList);

            // Final Result
            var onBoardingResponse = new OnBoardingResponse();
            onBoardingResponse.providers = provider;
            onBoardingResponse.drivers = driver;
            onBoardingResponse.providerAndDrivers = combined;

            return onBoardingResponse;
        }
    }
}
