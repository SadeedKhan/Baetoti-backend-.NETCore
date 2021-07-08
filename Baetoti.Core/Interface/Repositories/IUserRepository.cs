﻿using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Response.User;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByMobileNumberAsync(string mobileNumber);
        Task<OnBoardingResponse> GetOnBoardingDataAsync();
        Task<UserResponse> GetAllUsersDataAsync();
        Task<UserResponse> GetFilteredUsersDataAsync(FilterRequest filterRequest);
    }
}
