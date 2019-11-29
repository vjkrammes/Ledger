using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LedgerLib.Interfaces
{
    public interface IHistoryDAL<T> where T : class, new()
    {
        int Count { get; }
        IEnumerable<T> Get(Expression<Func<T, bool>> pred = null);
    }
}
