using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Transaction;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Baetoti.Shared.Enum;
using System;
using Baetoti.Shared.Request.Transaction;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class TransactionRepository : EFRepository<Transaction>, ITransactionRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public TransactionRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AllTransactions>> GetAll()
        {
            return await (from t in _dbContext.Transactions
                          join u in _dbContext.Users on t.UserID equals u.ID
                          join po in _dbContext.ProviderOrders on t.OrderID equals po.OrderID
                          join p in _dbContext.Users on po.ProviderID equals p.ID
                          select new AllTransactions
                          {
                              TransactionID = t.ID,
                              UserID = u.ID,
                              OrderID = t.OrderID,
                              TransactionAmount = t.Amount,
                              TransactionFrom = $"{u.FirstName} {u.LastName}",
                              TransactionTo = $"{p.FirstName} {p.LastName}",
                              TransactionFor = "",
                              TransactionStatus = Convert.ToString((TransactionStatus)t.Status),
                              PaymentType = Convert.ToString((TransactionType)t.TransactionType),
                              TransactionTime = t.TransactionTime
                          }).ToListAsync();
        }

        public async Task<List<AllTransactions>> GetFitered(TransactionFilterRequest transactionFilterRequest)
        {
            return await (from t in _dbContext.Transactions
                          join u in _dbContext.Users on t.UserID equals u.ID
                          join po in _dbContext.ProviderOrders on t.OrderID equals po.OrderID
                          join p in _dbContext.Users on po.ProviderID equals p.ID
                          select new AllTransactions
                          {
                              TransactionID = t.ID,
                              UserID = u.ID,
                              OrderID = t.OrderID,
                              TransactionAmount = t.Amount,
                              TransactionFrom = $"{u.FirstName} {u.LastName}",
                              TransactionTo = $"{p.FirstName} {p.LastName}",
                              TransactionFor = "",
                              TransactionStatus = Convert.ToString((TransactionStatus)t.Status),
                              PaymentType = Convert.ToString((TransactionType)t.TransactionType),
                              TransactionTime = t.TransactionTime
                          }).ToListAsync();
        }

        public async Task<TransactionResponseByID> GetByID(long Id)
        {
            return await (from t in _dbContext.Transactions
                          join u in _dbContext.Users on t.UserID equals u.ID
                          join po in _dbContext.ProviderOrders on t.OrderID equals po.OrderID
                          join p in _dbContext.Users on po.ProviderID equals p.ID

                          where t.ID == Id
                          select new TransactionResponseByID
                          {
                              TransactionID = t.ID,
                              UserID = u.ID,
                              OrderID = t.OrderID,
                              TransactionAmount = t.Amount,
                              TransactionFrom = $"{u.FirstName} {u.LastName}",
                              TransactionTo = $"{p.FirstName} {p.LastName}",
                              TransactionFor = "Order",
                              PaymentType = Convert.ToString((TransactionType)t.TransactionType),
                              TransactionTime = t.TransactionTime,
                              ProviderInvoice = "#2Av34dDR43sdicxDkx32ST",
                              DriverInvoice = "#Dkx32STdDR43s2Av34dicZ",
                              UserInvoice = "#R4Av34d3skSTdix32c2DxD"
                          }).FirstOrDefaultAsync();
        }

    }
}
