using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Base;
using Baetoti.Shared.Response.Invoice;
using System.Threading.Tasks;

namespace Baetoti.Core.Interface.Repositories
{
    public interface IProviderRepository : IAsyncRepository<Provider>
    {
        Task<Provider> GetByUserID(long UserID);

        Task<InvoiceResponse> GetProviderInvoice(long OrderID,int UserTypeID);

    }
}
