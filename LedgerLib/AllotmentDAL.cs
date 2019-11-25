using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LedgerLib.Entities;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

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
            var allotments = _dbset.Where(x => x.PoolId == pid);
            if (allotments.Any())
            {
                _dbset.RemoveRange(allotments);
                _context.SaveChanges();
            }
        }

        public override IEnumerable<AllotmentEntity> Get(Expression<Func<AllotmentEntity, bool>> pred = null)
        {
            return pred switch
            {
                null => _dbset
                        .Include(x => x.Company)
                        .OrderByDescending(x => x.Date)
                        .Select(x => x).AsNoTracking().ToList(),
                _ => _dbset
                        .Include(x => x.Company)
                        .Where(pred)
                        .OrderByDescending(x => x.Date)
                        .Select(x => x).AsNoTracking().ToList()
            };
        }

        public IEnumerable<AllotmentEntity> GetForPool(int pid) => Get(x => x.PoolId == pid);

        public IEnumerable<AllotmentEntity> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public AllotmentEntity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public bool PoolHasAllotments(int pid) => GetForPool(pid).Any();

        public bool CompanyHasAllotments(int cid) => GetForCompany(cid).Any();
    }
}
