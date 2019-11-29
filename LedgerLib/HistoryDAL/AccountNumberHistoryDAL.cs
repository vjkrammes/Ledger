using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib.HistoryDAL
{
    public class AccountNumberHistoryDAL : HistoryDAL<AccountNumberHistoryEntity, HistoryContext>, IAccountNumberHistoryDAL
    {
        public AccountNumberHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<AccountNumberHistoryEntity> Get(Expression<Func<AccountNumberHistoryEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset.OrderByDescending(x => x.EndDate).AsNoTracking().ToList(),
                _ => _dbset.Where(pred).OrderByDescending(x => x.EndDate).AsNoTracking().ToList()
            };
        }

        public IEnumerable<AccountNumberHistoryEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public AccountNumberHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountNumberHistoryEntity Current(int aid) => 
            Get(x => x.AccountId == aid && x.EndDate == DateTime.MaxValue).SingleOrDefault();
    }
}
