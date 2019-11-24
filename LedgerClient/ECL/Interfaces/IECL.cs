using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LedgerClient.ECL.Interfaces
{
    public interface IECL<Tentity, TDTO>
    {
        int Count { get; }
        void Insert(TDTO dto);
        void Update(TDTO dto);
        void Delete(TDTO dto);
        IEnumerable<TDTO> Get(Expression<Func<Tentity, bool>> pred = null);
    }
}
