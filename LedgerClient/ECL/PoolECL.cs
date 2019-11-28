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
    public class PoolECL : IPoolECL
    {
        private readonly IMapper _mapper;

        public PoolECL(IMapper mapper) => _mapper = mapper;

        public int Count { get => Tools.Locator.PoolDAL.Count; }

        public void Insert(Pool dto)
        {
            PoolEntity entity = _mapper.Map<PoolEntity>(dto);
            Tools.Locator.PoolDAL.Insert(entity);
            dto.Id = entity.Id;
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Update(Pool dto)
        {
            PoolEntity entity = _mapper.Map<PoolEntity>(dto);
            Tools.Locator.PoolDAL.Update(entity);
            dto.RowVersion = entity.RowVersion.ArrayCopy();
        }

        public void Delete(Pool dto)
        {
            PoolEntity entity = _mapper.Map<PoolEntity>(dto);
            Tools.Locator.PoolDAL.Delete(entity);
        }

        public IEnumerable<Pool> Get(Expression<Func<PoolEntity, bool>> pred = null)
        {
            var entities = Tools.Locator.PoolDAL.Get(pred);
            return _mapper.Map<List<Pool>>(entities);
        }

        public Pool Read(int id) => Get(x => x.Id == id).SingleOrDefault();

        public Pool Read(string name) => Get(x => x.Name == name).SingleOrDefault();
    }
}
