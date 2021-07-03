﻿using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByMobileNumberAsync(string mobileNumber);
    }
}
