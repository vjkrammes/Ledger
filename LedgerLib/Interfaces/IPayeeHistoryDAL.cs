using LedgerLib.HistoryEntities;

namespace LedgerLib.Interfaces
{
    public interface IPayeeHistoryDAL : IHistoryDAL<PayeeHistoryEntity>
    {
        PayeeHistoryEntity Read(int id);
        PayeeHistoryEntity Read(string name);
    }
}
