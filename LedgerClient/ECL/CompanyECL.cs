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
    public class CompanyECL : ICompanyECL
    {
        private readonly IMapper _mapper;

        public CompanyECL(IMapper mapper) => _mapper = mapper;

        public int Count => Tools.Locator.CompanyDAL.Count;

        public void Insert(Company dto)
        {
            var entity = _mapper.Map<CompanyEntity>(dto);
            Tools.Locator.CompanyDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Company dto)
        {
            var entity = _mapper.Map<CompanyEntity>(dto);
            Tools.Locator.CompanyDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(Company dto)
        {
            var entity = _mapper.Map<CompanyEntity>(dto);
            Tools.Locator.CompanyDAL.Delete(entity);
        }

        public IEnumerable<Company> Get(Expression<Func<CompanyEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.CompanyDAL.Get(pred);
            return _mapper.Map<List<Company>>(entities);
        }

        public IEnumerable<Company> GetPayees() => Get(x => x.IsPayee);

        public Company Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public Company Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
