using Baetoti.Core.Interface.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IUnitRepository : IAsyncRepository<Unit>
    {
        Task<Unit> AddAsync(Unit entity);

        Task UpdateAsync(Unit entity);

        Task<Unit> GetByIdAsync(int id);

        Task<IReadOnlyList<Unit>> ListAllAsync();

        Task DeleteAsync(Unit entity);
    }
}
