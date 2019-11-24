using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerLib
{
    public class AccountDAL : DAL<AccountEntity, LedgerContext>, IAccountDAL
    {
        public AccountDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<AccountEntity> Get(Expression<Func<AccountEntity, bool>> pred = null)
        {
            List<AccountEntity> entities = pred switch
            {
                null => _dbset
                        .Include(x => x.AccountType)
                        .Select(x => x).AsNoTracking().ToList(),
                _ => _dbset
                        .Include(x => x.AccountType)
                        .Where(pred)
                        .Select(x => x).AsNoTracking().ToList()
            };
            DateTime now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.AccountNumber = _context.AccountNumbers
                    .Where(x => x.AccountId == entity.Id && now >= x.StartDate && now <= x.StopDate).SingleOrDefault();
            }
            return entities;
        }

        public IEnumerable<AccountEntity> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public IEnumerable<AccountEntity> GetForAccountType(int atid) => Get(x => x.AccountTypeId == atid);

        public AccountEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public bool CompanyHasAccounts(int cid) => GetForCompany(cid).Any();

        public bool AccountTypeHasAccounts(int atid) => GetForAccountType(atid).Any();
    }
}
