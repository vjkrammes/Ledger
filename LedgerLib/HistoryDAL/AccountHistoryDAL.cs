using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.HistoryEntities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib.HistoryDAL
{
    public class AccountHistoryDAL : HistoryDAL<AccountHistoryEntity, HistoryContext>, IAccountHistoryDAL
    {

        public AccountHistoryDAL(HistoryContext context) : base(context) { }

        public override IEnumerable<AccountHistoryEntity> Get(Expression<Func<AccountHistoryEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset
                    .Include(x => x.AccountType)
                    .Include(x => x.AccountNumbers)
                    .AsNoTracking().ToList(),
                _ => _dbset
                    .Include(x => x.AccountType)
                    .Include(x => x.AccountNumbers)
                    .Where(pred)
                    .AsNoTracking().ToList()
            };
        }

        public IEnumerable<AccountHistoryEntity> GetForPayee(int pid) => Get(x => x.PayeeId == pid);

        public IEnumerable<AccountHistoryEntity> GetForAccountType(int atid) => Get(x => x.AccountTypeId == atid);

        public AccountHistoryEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();
    }
}
