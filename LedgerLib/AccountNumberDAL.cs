using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class AccountNumberDAL : DAL<AccountNumberEntity, LedgerContext>, IAccountNumberDAL
    {
        public AccountNumberDAL(LedgerContext context) : base(context) { }

        public void DeleteForAccount(int aid)
        {
            var numbers = DbSet.Where(x => x.AccountId == aid).ToList();
            DbSet.RemoveRange(numbers);
            Context.SaveChanges();
        }

        public override IEnumerable<AccountNumberEntity> Get(Expression<Func<AccountNumberEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .OrderByDescending(x => x.StopDate)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Where(pred)
                        .OrderByDescending(x => x.StopDate)
                        .AsNoTracking().ToList()
            };

        public IEnumerable<AccountNumberEntity> GetForAccount(int aid) => Get(x => x.AccountId == aid);

        public AccountNumberEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountNumberEntity Current(int aid)
        {
            var now = DateTime.Now;
            return Get(x => x.AccountId == aid && now >= x.StartDate && now <= x.StopDate).SingleOrDefault();
        }

        public bool AccountHasAccountNumbers(int aid) => GetForAccount(aid).Any();
    }
}
