using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;
using LedgerClient.Views;

using LedgerLib.Infrastructure;
using LedgerLib.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace LedgerClient.ViewModels
{
    public partial class MainViewModel
    {
        public override void Cancel()
        {
            Application.Current.MainWindow.Close();
        }

        #region Company Methods

        private void AddCompanyClick()
        {
            var vm = Tools.Locator.CompanyViewModel;
            if (DialogSupport.ShowDialog<CompanyWindow>(vm, Application.Current.MainWindow) != true)
            {
                return;
            }
            Company c = vm.Company.Clone();
            c.Name = c.Name.Caseify();
            try
            {
                Tools.Locator.CompanyECL.Insert(c);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Company", "Add");
                return;

            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to add new Company", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            int ix = 0;
            while (ix < Companies.Count && Companies[ix] < c)
                ix++;
            Companies.Insert(ix, c);
            SelectedCompany = c;
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool CompanySelected() => SelectedCompany != null;

        private void EditCompanyClick()
        {
            if (SelectedCompany is null)
            {
                return;
            }
            var vm = Tools.Locator.CompanyViewModel;
            vm.Company = SelectedCompany.Clone();
            if (DialogSupport.ShowDialog<CompanyWindow>(vm, Application.Current.MainWindow) != true)
            {
                return;
            }
            if (!vm.Company.Name.Equals(SelectedCompany.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (Tools.Locator.CompanyECL.Read(vm.Company.Name) != null)
                {
                    PopupManager.Popup($"A Company with the name '{vm.Company.Name}' already exists", "Duplicate Company",
                        PopupButtons.Ok, PopupImage.Stop);
                    return;
                }
            }
            Company c = vm.Company.Clone();
            c.Name = c.Name.Caseify();
            try
            {
                Tools.Locator.CompanyECL.Update(c);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Company", "Update");
                LoadCompanies(true);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Company", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            Companies.Remove(SelectedCompany);
            SelectedCompany = null;
            int ix = 0;
            while (ix < Companies.Count && Companies[ix] < c)
                ix++;
            Companies.Insert(ix, c);
            SelectedCompany = c;
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool DeleteCompanyCanClick() => SelectedCompany != null && CompanyCanBeDeleted(SelectedCompany.Id);

        private void DeleteCompanyClick()
        {
            if (SelectedCompany is null || !CompanyCanBeDeleted(SelectedCompany.Id))
            {
                return;
            }
            string msg = $"Delete Company '{SelectedCompany.Name}'? This action cannot be undone.";
            if (PopupManager.Popup(msg, "Delete Company?", PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
            {
                return;
            }
            try
            {
                Tools.Locator.CompanyECL.Delete(SelectedCompany);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Company", "Delete");
                LoadCompanies(true);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Company", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            Companies.Remove(SelectedCompany);
            if (Companies.Any())
            {
                SelectedCompany = Companies[0];
            }
            else
            {
                SelectedCompany = null;
            }
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        #endregion

        #region Account Methods

        private void AddAccountClick()
        {
            if (SelectedCompany is null)
            {
                return;
            }
            var vm = Tools.Locator.AccountViewModel;
            vm.Company = SelectedCompany;
            if (DialogSupport.ShowDialog<AccountWindow>(vm, Application.Current.MainWindow) != true)
            {
                return;
            }
            Account a = new Account
            {
                CompanyId = SelectedCompany.Id,
                AccountTypeId = vm.SelectedType.Id,
                DueDateType = vm.SelectedDueDateType,
                Month = vm.Month,
                Day = vm.Day,
                IsPayable = vm.IsPayable,
                Comments = vm.Comments ?? string.Empty,
                AccountType = vm.SelectedType
            };
            try
            {
                a = Tools.Locator.AccountECL.Create(a, vm.Number);
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to create new Account", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            Accounts.Insert(0, a);
            SelectedAccount = a;
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool AccountSelected() => SelectedAccount != null;

        private void EditAccountClick()
        {
            if (SelectedAccount is null)
            {
                return;
            }
            var vm = Tools.Locator.AccountViewModel;
            vm.Company = SelectedCompany;
            vm.Account = SelectedAccount;
            if (DialogSupport.ShowDialog<AccountWindow>(vm, Application.Current.MainWindow) != true)
            {
                LoadAccounts(); // account number may have changed
                return;
            }
            Account save = SelectedAccount.Clone();
            SelectedAccount.AccountTypeId = vm.SelectedType.Id;
            SelectedAccount.AccountType = vm.SelectedType;
            SelectedAccount.DueDateType = vm.SelectedDueDateType;
            SelectedAccount.Month = vm.Month;
            SelectedAccount.Day = vm.Day;
            SelectedAccount.Comments = vm.Comments ?? string.Empty;
            SelectedAccount.IsPayable = vm.IsPayable;
            try
            {
                Tools.Locator.AccountECL.Update(SelectedAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                SelectedAccount = save.Clone();
                Tools.ConcurrencyError("Account", "Update");
            }
            LoadAccounts(true);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool ViewHistoryCanClick() => 
            SelectedAccount != null && Tools.Locator.AccountNumberECL.AccountHasAccountNumbers(SelectedAccount.Id);

        private void ViewHistoryClick()
        {
            if (SelectedAccount is null)
            {
                return;
            }
            var vm = Tools.Locator.NumberHistoryViewModel;
            vm.Account = SelectedAccount;
            DialogSupport.ShowDialog<NumberHistoryWindow>(vm, Application.Current.MainWindow);
        }

        private bool DeleteAccountCanClick() =>
            SelectedAccount != null && !Tools.Locator.TransactionECL.AccountHasTransactions(SelectedAccount.Id);

        private void DeleteAccountClick()
        {
            if (!DeleteAccountCanClick())
            {
                return;
            }
            int accountid = SelectedAccount.Id;
            if (Tools.Locator.AccountNumberECL.AccountHasAccountNumbers(SelectedAccount.Id))
            {
                string msg = "Delete this account? All Account Number history will be lost.";
                if (PopupManager.Popup("Delete This Account?","Delete Account?",msg,PopupButtons.YesNo, PopupImage.Question) !=
                    PopupResult.Yes)
                {
                    return;
                }
            }
            try
            {
                Tools.Locator.AccountECL.Delete(SelectedAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Account", "Delete");
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Account", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            Accounts.Remove(SelectedAccount);
            SelectedAccount = Accounts.Any() ? Accounts[0] : null;
            try
            {
                Tools.Locator.AccountNumberECL.DeleteForAccount(accountid);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Account Number", "Delete");
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Account Numbers", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        #endregion

        #region Transaction Methods

        private bool AddTransactionCanClick() => SelectedAccount != null && SelectedAccount.IsPayable;

        private void AddTransactionClick()
        {
            if (!AddTransactionCanClick())
            {
                return;
            }
            var vm = Tools.Locator.TransactionViewModel;
            if (DialogSupport.ShowDialog<TransactionWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedTransaction = null;
                return;
            }
            if (vm.Payment > vm.Balance)
            {
                PopupManager.Popup("Payment is greater than Balance", "Invalid Transaction", PopupButtons.Ok, PopupImage.Stop);
                SelectedTransaction = null;
                return;
            }
            Transaction t = new Transaction
            {
                AccountId = SelectedAccount.Id,
                Date = vm.Date ?? (default),
                Balance = vm.Balance,
                Payment = vm.Payment,
                Reference = vm.Reference ?? string.Empty
            };
            try
            {
                Tools.Locator.TransactionECL.Insert(t);
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to insert new Transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            AddTransaction(t);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool TransactionSelected() => SelectedTransaction != null;

        private void ChangeDateClick()
        {
            if (SelectedTransaction is null)
            {
                return;
            }
            var vm = Tools.Locator.DateViewModel;
            vm.Prompt = "Transaction Date:";
            vm.Date = SelectedTransaction.Date;
            vm.Border = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            vm.Validator = ValidateDate;
            if (DialogSupport.ShowDialog<DateWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedTransaction = null;
                return;
            }
            Transaction t = SelectedTransaction.Clone();
            t.Date = vm.Date.Value;
            try
            {
                Tools.Locator.TransactionECL.Update(t);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Transaction", "Update");
                SelectedTransaction = null;
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            Transactions.Remove(t);
            AddTransaction(t);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private void ChangeBalanceClick()
        {
            if (SelectedTransaction is null)
            {
                return;
            }
            var vm = Tools.Locator.QAViewModel;
            vm.Question = "Balance:";
            vm.Answer = SelectedTransaction.Balance.ToString();
            vm.AnswerRequired = true;
            vm.BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            vm.Validator = ValidateMoney;
            if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedTransaction = null;
                return;
            }
            if (string.IsNullOrEmpty(vm.Answer) || !decimal.TryParse(vm.Answer, out decimal newbalance))
            {
                SelectedTransaction = null;
                return;
            }
            if (newbalance < SelectedTransaction.Payment)
            {
                PopupManager.Popup("New Balance is less than Payment", "Invalid Balance", PopupButtons.Ok, PopupImage.Stop);
                SelectedTransaction = null;
                return;
            }
            Transaction t = SelectedTransaction.Clone();
            t.Balance = newbalance;
            try
            {
                Tools.Locator.TransactionECL.Update(t);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Transaction", "Update");
                SelectedTransaction = null;
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            Transactions.Remove(SelectedTransaction);
            AddTransaction(t);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private void ChangePaymentClick()
        {
            if (SelectedTransaction is null)
            {
                return;
            }
            var vm = Tools.Locator.QAViewModel;
            vm.Question = "Payment:";
            vm.Answer = SelectedTransaction.Payment.ToString();
            vm.AnswerRequired = true;
            vm.BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            vm.Validator = ValidateMoney;
            if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedTransaction = null;
                return;
            }
            if (string.IsNullOrEmpty(vm.Answer) || !decimal.TryParse(vm.Answer, out decimal newpayment))
            {
                SelectedTransaction = null;
                return;
            }
            Transaction t = SelectedTransaction.Clone();
            t.Payment = newpayment;
            try
            {
                Tools.Locator.TransactionECL.Update(t);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Transaction", "Update");
                SelectedTransaction = null;
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            Transactions.Remove(SelectedTransaction);
            AddTransaction(t);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private void ChangeReferenceClick()
        {
            if (SelectedTransaction is null)
            {
                return;
            }
            var vm = Tools.Locator.QAViewModel;
            vm.Question = "Reference:";
            vm.Answer = SelectedTransaction.Reference ?? string.Empty;
            vm.AnswerRequired = false;
            vm.BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush;
            if (DialogSupport.ShowDialog<QAWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedTransaction = null;
                return;
            }
            Transaction t = SelectedTransaction.Clone();
            t.Reference = vm.Answer ?? string.Empty;
            try
            {
                Tools.Locator.TransactionECL.Update(t);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Transaction", "Update");
                SelectedTransaction = null;
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            Transactions.Remove(SelectedTransaction);
            AddTransaction(t);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private void DeselectTransactionClick() => SelectedTransaction = null;

        private void DeleteTransactionClick()
        {
            if (SelectedTransaction is null)
            {
                return;
            }
            try
            {
                Tools.Locator.TransactionECL.Delete(SelectedTransaction);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Transaction", "Update");
                SelectedTransaction = null;
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete transaction", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedTransaction = null;
                return;
            }
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        #endregion

        #region Identity Methods

        private bool IdentitySelected() => SelectedIdentity != null;

        private void CopyURLClick()
        {
            if (SelectedIdentity is null)
            {
                return;
            }
            Clipboard.SetText(SelectedIdentity.URL);
        }

        private bool CopyPasswordCanClick() => SelectedIdentity != null && !string.IsNullOrEmpty(SelectedIdentity.Password);

        private void CopyPasswordClick()
        {
            if (SelectedIdentity is null || string.IsNullOrEmpty(SelectedIdentity.Password))
            {
                return;
            }
            Clipboard.SetText(Tools.Locator.StringCypher.Decrypt(SelectedIdentity.Password, Tools.Locator.PasswordManager.Get(),
                SelectedIdentity.PasswordSalt));
            Tools.Locator.StatusbarViewModel.LastCopyDate = DateTime.Now;
        }

        private void ClearClipboardClick()
        {
            Clipboard.Clear();
            Tools.Locator.StatusbarViewModel.LastCopyDate = default;
        }

        private void AddIdentityClick()
        {
            if (SelectedCompany is null)
            {
                return;
            }
            var vm = Tools.Locator.IdentityViewModel;
            vm.Company = SelectedCompany.Clone();
            if (DialogSupport.ShowDialog<IdentityWindow>(vm, Application.Current.MainWindow) != true)
            {
                return;
            }
            Identity i = vm.Identity.Clone();
            try
            {
                Tools.Locator.IdentityECL.Insert(i);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Identity", "Insert");
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to insert new Identity", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            Identities.Insert(0, i);
            SelectedIdentity = i;
            SelectedIdentity = null;
            Tools.Locator.StatusbarViewModel.Update();
        }

        private void EditIdentityClick()
        {
            if (SelectedIdentity is null)
            {
                return;
            }
            var vm = Tools.Locator.IdentityViewModel;
            vm.Identity = SelectedIdentity.Clone();
            if (DialogSupport.ShowDialog<IdentityWindow>(vm, Application.Current.MainWindow) != true)
            {
                SelectedIdentity = null;
                return;
            }
            Identity i = vm.Identity.Clone();
            try
            {
                Tools.Locator.IdentityECL.Update(i);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Identity", "Update");
                LoadIdentities(true);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to update Identity", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedIdentity = null;
                return;
            }
            Identities.Remove(SelectedIdentity);
            Identities.Insert(0, i);
            SelectedIdentity = i;
            SelectedIdentity = null;
        }

        private void DeselectIdentityClick() => SelectedIdentity = null;

        private void DeleteIdentityClick()
        {
            if (SelectedIdentity is null)
            {
                return;
            }
            var user = Tools.Locator.StringCypher.Decrypt(SelectedIdentity.UserId, Tools.Locator.PasswordManager.Get(),
                SelectedIdentity.UserSalt);
            string msg = $"Delete Identity '{user}' from Company {SelectedIdentity.Company.Name}?";
            if (PopupManager.Popup(msg, "Delete Identity?", PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
            {
                SelectedIdentity = null;
                return;
            }
            try
            {
                Tools.Locator.IdentityECL.Delete(SelectedIdentity);
            }
            catch (DbUpdateConcurrencyException)
            {
                Tools.ConcurrencyError("Identity", "Delete");
                LoadIdentities(true);
                return;
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Failed to delete Identity", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedIdentity = null;
                return;
            }
            Identities.Remove(SelectedIdentity);
            SelectedIdentity = null;
            Tools.Locator.StatusbarViewModel.Update();
        }

        #endregion

        #region Manage Methods

        private void ManageAccountTypesClick()
        {
            AccountTypeViewModel vm = Tools.Locator.AccountTypeViewModel;
            DialogSupport.ShowDialog<AccountTypeWindow>(vm, Application.Current.MainWindow);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private void ManagePoolsClick()
        {
            var vm = Tools.Locator.PoolViewModel;
            DialogSupport.ShowDialog<PoolWindow>(vm, Application.Current.MainWindow);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

        private bool CleanUpOrphanedAccountNumbersCanClick() => OrphanedAccountNumbers != null && OrphanedAccountNumbers.Any();

        private void CleanUpOrphanedAccountNumbersClick()
        {
            if (!OrphanedAccountNumbers.Any())
            {
                return;
            }
            using (var wc = new WaitCursor())
            {
                foreach (var accountnumber in OrphanedAccountNumbers)
                {
                    try
                    {
                        Tools.Locator.AccountNumberECL.DeleteForAccount(accountnumber);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Tools.ConcurrencyError("Account Number", "Delete");
                    }
                    catch (Exception ex)
                    {
                        PopupManager.Popup($"Failed to delete Account Numbers for Account #{accountnumber}", Constants.DBE,
                            ex.Innermost(), PopupButtons.Ok, PopupImage.Warning);
                    }
                }
            }
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
            OrphanedAccountNumbers = new ObservableCollection<int>(GetOrphanedAccountNumbers());
            if (OrphanedAccountNumbers.Any())
            {
                PopupManager.Popup("Some Orphaned account numbers remain", "Not All Deleted", PopupButtons.Ok, PopupImage.Information);
            }
            else
            {
                PopupManager.Popup("All Orphaned Account Numbers have been Deleted", "All Deleted", PopupButtons.Ok,
                    PopupImage.Information);
            }
        }

        #endregion

        #region Ledger5 History Methods

        private bool HistoryExists() => Tools.Locator.LedgerContext.DatabaseExists(Constants.DefaultHistoryDatabase) == true;

        private void HistoryClick()
        {
            //var vm = Tools.Locator.HistoryViewModel;
        }

        #endregion

        #region Miscellaneous Methods

        private void IconHeight(object parm)
        {
            if (parm is string h && double.TryParse(h, out double height))
            {
                Tools.Locator.Settings.IconHeight = height;
                Application.Current.Resources[Constants.IconHeight] = height;
            }
        }

        private void ToggleStatusbar()
        {
            Tools.Locator.Settings.ShowStatusbar = !Tools.Locator.Settings.ShowStatusbar;
            StatusbarVisibility = Tools.Locator.Settings.ShowStatusbar ? Visibility.Visible : Visibility.Collapsed;
            Tools.Locator.StatusbarViewModel.StatusbarVisibility = StatusbarVisibility;
        }

        private void BackupClick()
        {
            DialogSupport.ShowDialog<BackupWindow>(Tools.Locator.BackupViewModel, Application.Current.MainWindow);
        }

        private void PalletteClick()
        {
            DialogSupport.ShowDialog<PalletteWindow>(Tools.Locator.PalletteViewModel, Application.Current.MainWindow);
        }

        private void AboutClick()
        {
            DialogSupport.ShowDialog<AboutWindow>(Tools.Locator.AboutViewModel, Application.Current.MainWindow);
        }

        #endregion

        #region Window Loaded

        private void WindowLoaded()
        {
            ISettingsService settings = Tools.Locator.Settings;
            WindowTitle = $"{settings.ProductName} Version {settings.ProductVersion:0.00}";
            Banner = $"{settings.ProductName} {settings.ProductVersion:0.00} - Manage your Accounts and Identities";
            ShortTitle = Tools.GetShortTitle(settings);
            StatusbarVisibility = settings.ShowStatusbar ? Visibility.Visible : Visibility.Collapsed;
            Tools.Locator.StatusbarViewModel.StatusbarVisibility = StatusbarVisibility;
            Authenticate(settings);
            LoadCompanies(false);
            Tools.Locator.StatusbarViewModel.Update();
            OrphanedAccountNumbers = new ObservableCollection<int>(GetOrphanedAccountNumbers());
            if (OrphanedAccountNumbers.Any())
            {
                string msg = "Orphaned Account Numbers can be removed by using the button on the tool bar";
                PopupManager.Popup("Orphaned Account Numbers Exist.", "Orphaned Account Numbers", msg, PopupButtons.Ok, 
                    PopupImage.Information);
            }
        }

        #endregion
    }
}
