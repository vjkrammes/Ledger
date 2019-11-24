using System.Collections.Generic;

using LedgerClient.ECL.DTO;

using LedgerLib.Entities;

namespace LedgerClient.ECL.Interfaces
{
    public interface ICompanyECL : IECL<CompanyEntity, Company>
    {
        IEnumerable<Company> GetPayees();
        Company Read(int id);
        Company Read(string name);
    }
}
