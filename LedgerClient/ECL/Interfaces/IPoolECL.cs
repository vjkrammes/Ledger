using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface IPoolECL : IECL<PoolEntity, Pool>
    {
        Pool Read(int id);
        Pool Read(string name);
    }
}
