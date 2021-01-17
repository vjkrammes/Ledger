using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class IdentityDAL : DAL<IdentityEntity, LedgerContext>, IIdentityDAL
    {
        public IdentityDAL(LedgerContext context) : base(context) { }

        public override void Insert(IdentityEntity entity)
        {
            entity.Company = null;      // ef will try to add it
            base.Insert(entity);
        }

        public override IEnumerable<IdentityEntity> Get(Expression<Func<IdentityEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .Include(x => x.Company)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Include(x => x.Company)
                        .Where(pred)
                        .AsNoTracking().ToList()
            };

        public IEnumerable<IdentityEntity> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public IdentityEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public IdentityEntity Read(int cid, string url) => Get(x => x.CompanyId == cid && x.URL == url).SingleOrDefault();

        public bool CompanyHasIdentities(int cid) => GetForCompany(cid).Any();
    }
}
