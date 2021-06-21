using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<Category> AddAsync(Category entity);

        Task UpdateAsync(Category entity);

        Task<Category> GetByIdAsync(int id);

        Task<IReadOnlyList<Category>> ListAllAsync();

        Task DeleteAsync(Category entity);
    }
}
