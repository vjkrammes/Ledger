using AutoMapper;

using LedgerClient.ECL.DTO;
using LedgerClient.ECL.Interfaces;
using LedgerClient.Infrastructure;

using LedgerLib.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LedgerClient.ECL
{
    public class IdentityECL : IIdentityECL
    {
        private readonly IMapper _mapper;

        public IdentityECL(IMapper mapper) => _mapper = mapper;

        public int Count => Tools.Locator.IdentityDAL.Count;

        public void Insert(Identity dto)
        {
            var entity = _mapper.Map<IdentityEntity>(dto);
            Tools.Locator.IdentityDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Identity dto)
        {
            var entity = _mapper.Map<IdentityEntity>(dto);
            Tools.Locator.IdentityDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }
        
        public void Delete(Identity dto)
        {
            var entity = _mapper.Map<IdentityEntity>(dto);
            Tools.Locator.IdentityDAL.Delete(entity);
        }

        public IEnumerable<Identity> Get(Expression<Func<IdentityEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.IdentityDAL.Get(pred);
            return _mapper.Map<List<Identity>>(entities);
        }

        public IEnumerable<Identity> GetForCompany(int id) => Get(x => x.CompanyId == id);

        public Identity Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public Identity Read(int cid, string url) => Get(x => x.CompanyId == cid && x.URL == url).SingleOrDefault();

        public bool CompanyHasIdentities(int cid) => GetForCompany(cid).Any();
    }
}
