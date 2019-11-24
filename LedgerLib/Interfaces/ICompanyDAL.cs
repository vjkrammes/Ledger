using System.Collections.Generic;

using LedgerLib.Entities;

namespace LedgerLib.Interfaces
{
    public interface ICompanyDAL : IDAL<CompanyEntity>
    {
        IEnumerable<CompanyEntity> GetPayees();
        CompanyEntity Read(int id);
        CompanyEntity Read(string name);
    }
}
