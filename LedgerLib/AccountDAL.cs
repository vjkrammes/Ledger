using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class AccountDAL : DAL<AccountEntity, LedgerContext>, IAccountDAL
    {
        public AccountDAL(LedgerContext context) : base(context) { }

        public AccountEntity Create(AccountEntity account, byte[] salt, string number)
        {
            using var transaction = Context.Database.BeginTransaction();
            if (account.Id <= 0)                // insert account and create new accountnumber
            {
                var at = account.AccountType;
                account.AccountType = null;
                DbSet.Add(account);
                Context.SaveChanges();
                account.AccountType = at;
                var an = new AccountNumberEntity
                {
                    AccountId = account.Id,
                    StartDate = default,
                    StopDate = DateTime.MaxValue,
                    Salt = salt,
                    Number = number
                };
                Context.AccountNumbers.Add(an);
                Context.SaveChanges();
                account.AccountNumber = an;
            }
            else                                // update existing accountnumber and create a new one
            {
                var an = account.AccountNumber;
                an.StopDate = DateTime.Now;
                Context.AccountNumbers.Update(an);
                an = new AccountNumberEntity
                {
                    AccountId = account.Id,
                    StartDate = DateTime.Now,
                    StopDate = DateTime.MaxValue,
                    Salt = salt,
                    Number = number
                };
                Context.AccountNumbers.Add(an);
                Context.SaveChanges();
                account.AccountNumber = an;
            }
            transaction.Commit();
            return account;
        }

        public override IEnumerable<AccountEntity> Get(Expression<Func<AccountEntity, bool>> pred = null)
        {
            var entities = pred switch
            {
                null => DbSet
                        .Include(x => x.AccountType)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Include(x => x.AccountType)
                        .Where(pred)
                        .AsNoTracking().ToList()
            };
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                entity.AccountNumber = Context.AccountNumbers
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
