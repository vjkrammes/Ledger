using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class CompanyDAL : DAL<CompanyEntity, LedgerContext>, ICompanyDAL
    {
        public CompanyDAL(LedgerContext context) : base(context) { }

        public override IEnumerable<CompanyEntity> Get(Expression<Func<CompanyEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .OrderBy(x => x.Name)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Where(pred)
                        .OrderBy(x => x.Name)
                        .AsNoTracking().ToList()
            };

        public IEnumerable<CompanyEntity> GetPayees() => Get(x => x.IsPayee);

        public CompanyEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public CompanyEntity Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
