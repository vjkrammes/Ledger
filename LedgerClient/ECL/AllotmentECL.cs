using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;

using LedgerLib.Entities;

namespace LedgerClient.ECL
{
    public class AllotmentECL :  IAllotmentECL
    {
        private readonly IMapper _mapper;

        public AllotmentECL(IMapper mapper) => _mapper = mapper;

        public int Count { get => Tools.Locator.AllotmentDAL.Count; }

        public void Insert(Allotment dto)
        {
            AllotmentEntity entity = _mapper.Map<AllotmentEntity>(dto);
            Tools.Locator.AllotmentDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Allotment dto)
        {
            AllotmentEntity entity = _mapper.Map<AllotmentEntity>(dto);
            Tools.Locator.AllotmentDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(Allotment dto)
        {
            AllotmentEntity entity = _mapper.Map<AllotmentEntity>(dto);
            Tools.Locator.AllotmentDAL.Delete(entity);
        }

        public void DeleteAll(int pid) => Tools.Locator.AllotmentDAL.DeleteAll(pid);

        public IEnumerable<Allotment> Get(Expression<Func<AllotmentEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.AllotmentDAL.Get(pred);
            return _mapper.Map<List<Allotment>>(entities);
        }

        public IEnumerable<Allotment> GetForPool(int pid) => Get(x => x.PoolId == pid);

        public IEnumerable<Allotment> GetForCompany(int cid) => Get(x => x.CompanyId == cid);

        public Allotment Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public bool PoolHasAllotments(int pid) => GetForPool(pid).Any();

        public bool CompanyHasAllotments(int cid) => GetForCompany(cid).Any();
    }
}
