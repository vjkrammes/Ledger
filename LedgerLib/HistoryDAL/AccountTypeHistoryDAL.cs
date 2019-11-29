using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib.HistoryDAL
{
    public class AccountTypeHistoryDAL : HistoryDAL<AccountTypeHistoryEntity, HistoryContext>, IAccountTypeHistoryDAL
    {
        public AccountTypeHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<AccountTypeHistoryEntity> Get(Expression<Func<AccountTypeHistoryEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset.OrderBy(x => x.Description).AsNoTracking().ToList(),
                _ => _dbset.Where(pred).OrderBy(x => x.Description).AsNoTracking().ToList()
            };
        }

        public AccountTypeHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountTypeHistoryEntity Read(string description) => Get(x => x.Description == description).SingleOrDefault();
    }
}
