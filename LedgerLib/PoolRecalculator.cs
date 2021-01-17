using LedgerLib.Interfaces;

using System.Linq;

namespace LedgerLib
{
    public class PoolRecalculator : IPoolRecalculator
    {
        private readonly LedgerContext _context;

        public PoolRecalculator(LedgerContext context) => _context = context;

        public void Recalculate()
        {
            using var transaction = _context.Database.BeginTransaction();
            var pools = _context.Pools;
            foreach (var pool in pools)
            {
                var spent = (from a in _context.Allotments where a.PoolId == pool.Id select a).Sum(x => x.Amount);
                pool.Balance = pool.Amount - spent;
            }
            _context.SaveChanges();
            transaction.Commit();
        }
    }
}
