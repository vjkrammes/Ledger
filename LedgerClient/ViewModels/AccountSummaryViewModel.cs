using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;
using LedgerClient.Models;

using LedgerLib;

using Microsoft.EntityFrameworkCore;

using System.Collections.ObjectModel;
using System.Linq;

namespace LedgerClient.ViewModels
{
    public class AccountSummaryViewModel : ViewModelBase
    {
        #region Properties

        private ObservableCollection<AccountSummaryModel> _accounts;
        public ObservableCollection<AccountSummaryModel> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        private AccountSummaryModel _selectedAccount;
        public AccountSummaryModel SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }

        #endregion

        public AccountSummaryViewModel(LedgerContext context)
        {
            var cypher = Tools.Locator.StringCypher;
            var manager = Tools.Locator.PasswordManager;
            Accounts = new ObservableCollection<AccountSummaryModel>();
            var companies = context.Companies.OrderBy(x => x.Name).AsNoTracking().ToList();
            foreach (var company in companies)
            {
                var accounts = context.Accounts.Where(x => x.CompanyId == company.Id);
                foreach (var account in accounts)
                {
                    account.AccountType = context.AccountTypes.Where(x => x.Id == account.AccountTypeId).AsNoTracking().SingleOrDefault();
                    account.AccountNumber = context.AccountNumbers
                        .Where(x => x.AccountId == account.Id)
                        .OrderByDescending(x => x.StopDate)
                        .Take(1)
                        .AsNoTracking()
                        .ToList()
                        .SingleOrDefault();
                    var transaction = context.Transactions
                        .Where(x => x.AccountId == account.Id)
                        .OrderByDescending(x => x.Date)
                        .Take(1)
                        .AsNoTracking()
                        .ToList()
                        .SingleOrDefault();
                    Accounts.Add(new AccountSummaryModel(company, account, transaction, cypher, manager));
                }
            }
        }
    }
}
