using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Response.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IStoreRepository : IAsyncRepository<Store>
    {
        //Task<List<StoreResponse>> GetAll();
    }
}
