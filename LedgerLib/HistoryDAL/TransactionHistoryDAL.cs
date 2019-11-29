using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib.HistoryDAL
{
    public class TransactionHistoryDAL : HistoryDAL<TransactionHistoryEntity, HistoryContext>, ITransactionHistoryDAL
    {
        public TransactionHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<TransactionHistoryEntity> Get(Expression<Func<TransactionHistoryEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset.OrderByDescending(x => x.Date).AsNoTracking().ToList(),
                _ => _dbset.Where(pred).OrderByDescending(x => x.Date).AsNoTracking().ToList()
            };
        }

        public IEnumerable<TransactionHistoryEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public TransactionHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
