using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface ITagsRepository: IAsyncRepository<Tags>
    {
        Task<Tags> AddAsync(Tags entity);

        Task UpdateAsync(Tags entity);

        Task<Tags> GetByIdAsync(int id);

        Task<IReadOnlyList<Tags>> ListAllAsync();

        Task DeleteAsync(Tags entity);
    }
}
