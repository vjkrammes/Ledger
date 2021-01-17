using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerLib
{
    public class AllotmentDAL : DAL<AllotmentEntity, LedgerContext>, IAllotmentDAL
    {
        public AllotmentDAL(LedgerContext context) : base(context) { }

        public override void Insert(AllotmentEntity entity)
        {
            entity.Company = null;
            base.Insert(entity);
        }

        public void DeleteAll(int pid)
        {
            var allotments = DbSet.Where(x => x.PoolId == pid);
            if (allotments.Any())
            {
                DbSet.RemoveRange(allotments);
                Context.SaveChanges();
            }
        }

        public override IEnumerable<AllotmentEntity> Get(Expression<Func<AllotmentEntity, bool>> pred = null) => 
            pred switch
            {
                null => DbSet
                        .Include(x => x.Company)
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList(),
                _ => DbSet
                        .Include(x => x.Company)
                        .Where(pred)
                        .OrderByDescending(x => x.Date)
                        .AsNoTracking().ToList()
            };

        public IEnumerable<AllotmentEntity> GetForPool(int pid) => Get(x => x.PoolId == pid);

        public IEnumerable<AllotmentEntity> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public AllotmentEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public bool PoolHasAllotments(int pid) => GetForPool(pid).Any();

        public bool CompanyHasAllotments(int cid) => GetForCompany(cid).Any();
    }
}
