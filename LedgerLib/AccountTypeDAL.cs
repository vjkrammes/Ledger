using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class AccountTypeDAL : DAL<AccountTypeEntity, LedgerContext>, IAccountTypeDAL
    {
        public AccountTypeDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<AccountTypeEntity> Get(Expression<Func<AccountTypeEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .OrderBy(x => x.Description)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Where(pred)
                        .OrderBy(x => x.Description)
                        .AsNoTracking().ToList()
            };

        public AccountTypeEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public AccountTypeEntity Read(string description) => Get(x => x.Description == description).SingleOrDefault();
    }
}
