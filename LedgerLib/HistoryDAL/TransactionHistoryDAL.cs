using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib.HistoryDAL
{
    public class TransactionHistoryDAL : HistoryDAL<TransactionHistoryEntity, HistoryContext>, ITransactionHistoryDAL
    {
        public TransactionHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<TransactionHistoryEntity> Get(Expression<Func<TransactionHistoryEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet.OrderByDescending(x => x.Date).AsNoTracking().ToList(),
                _ => DbSet.Where(pred).OrderByDescending(x => x.Date).AsNoTracking().ToList()
            };

        public IEnumerable<TransactionHistoryEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public TransactionHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
