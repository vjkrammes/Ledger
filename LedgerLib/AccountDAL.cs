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

        public AccountEntity Create(AccountEntity account, byte[] salt, string number)
        {
            using var transaction = _context.Database.BeginTransaction();
            if (account.Id <= 0)                // insert account and create new accountnumber
            {
                var at = account.AccountType;
                account.AccountType = null;
                _dbset.Add(account);
                _context.SaveChanges();
                account.AccountType = at;
                AccountNumberEntity an = new AccountNumberEntity
                {
                    AccountId = account.Id,
                    StartDate = default,
                    StopDate = DateTime.MaxValue,
                    Salt = salt,
                    Number = number
                };
                _context.AccountNumbers.Add(an);
                _context.SaveChanges();
                account.AccountNumber = an;
            }
            else                                // update existing accountnumber and create a new one
            {
                AccountNumberEntity an = account.AccountNumber;
                an.StopDate = DateTime.Now;
                _context.AccountNumbers.Update(an);
                an = new AccountNumberEntity
                {
                    AccountId = account.Id,
                    StartDate = DateTime.Now,
                    StopDate = DateTime.MaxValue,
                    Salt = salt,
                    Number = number
                };
                _context.AccountNumbers.Add(an);
                _context.SaveChanges();
                account.AccountNumber = an;
            }
            transaction.Commit();
            return account;
        }

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
