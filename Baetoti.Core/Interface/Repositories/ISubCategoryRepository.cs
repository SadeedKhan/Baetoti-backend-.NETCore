using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface ISubCategoryRepository : IAsyncRepository<SubCategory>
    {
        Task<SubCategory> AddAsync(SubCategory entity);

        Task UpdateAsync(SubCategory entity);

        Task<SubCategory> GetByIdAsync(int id);

        Task<IReadOnlyList<SubCategory>> ListAllAsync();

        Task DeleteAsync(SubCategory entity);
    }
}
