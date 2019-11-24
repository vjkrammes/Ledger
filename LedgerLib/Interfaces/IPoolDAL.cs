using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface IPoolDAL : IDAL<PoolEntity>
    {
        PoolEntity Read(int id);
        PoolEntity Read(string name);
    }
}
