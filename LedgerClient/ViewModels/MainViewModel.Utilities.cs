﻿using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;
using LedgerClient.Views;

using LedgerLib.Infrastructure;
using LedgerLib.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace LedgerClient.ViewModels
{
    public partial class MainViewModel
    {
        private void Authenticate(ISettingsService settings)
        {
            if (settings.PasswordSalt is null && settings.PasswordHash is null)
            {
                CreateNewPassword(settings);
            }
            else
            {
                VerifyPassword(settings);
            }
        }

        private static void CreateNewPassword(ISettingsService settings)
        {
            var vm = Tools.Locator.PasswordViewModel;
            vm.Password2Visibility = Visibility.Visible;
            vm.ShortHeader = Tools.GetShortTitle();
            if (DialogSupport.ShowDialog<PasswordWindow>(vm, Application.Current.MainWindow) != true)
            {
                Environment.Exit(Constants.NoPasswordEntered);
            }
            var salt = Tools.GenerateSalt(Constants.SaltLength);
            var hash = Tools.GenerateHash(Encoding.UTF8.GetBytes(vm.Password1), salt, Constants.HashIterations, Constants.HashLength);
            settings.PasswordSalt = salt;
            settings.PasswordHash = hash;
        }

        private void VerifyPassword(ISettingsService settings)
        {
            var vm = Tools.Locator.PasswordViewModel;
            vm.Password2Visibility = Visibility.Collapsed;
            vm.Salt = settings.PasswordSalt;
            vm.Hash = settings.PasswordHash;
            vm.ShortHeader = Tools.GetShortTitle();
            if (DialogSupport.ShowDialog<PasswordWindow>(vm, Application.Current.MainWindow) != true)
            {
                Environment.Exit(Constants.NoPasswordEntered);
            }
            var hash = Tools.GenerateHash(Encoding.UTF8.GetBytes(vm.Password1), settings.PasswordSalt, Constants.HashIterations,
                        Constants.HashLength);
            if (!hash.ArrayEquals(settings.PasswordHash))
            {
                PopupManager.Popup("Incorrect Password Entered", "Incorrect Password", PopupButtons.Ok, PopupImage.Error);
                VerifyPassword(settings);
            }
        }

        private static bool CompanyCanBeDeleted(int cid)
        {
            if (Tools.Locator.IdentityECL.CompanyHasIdentities(cid))
            {
                return false;
            }
            if (Tools.Locator.AccountECL.CompanyHasAccounts(cid))
            {
                return false;
            }
            if (Tools.Locator.AllotmentECL.CompanyHasAllotments(cid))
            {
                return false;
            }
            return true;
        }

        private void LoadCompanies(bool reload = false)
        {
            SelectedCompany = null;
            try
            {
                Companies = new ObservableCollection<Company>(Tools.Locator.CompanyECL.Get());
            }
            catch (Exception ex)
            {
                var op = reload ? Constants.Reload : Constants.Load;
                PopupManager.Popup($"Failed to {op} Companies", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                Environment.Exit(Constants.CompaniesLoadFailed);
            }
            SelectedCompany = Companies.Any() ? Companies[0] : null;
        }

        private void LoadIdentities(bool reload = false)
        {
            SelectedIdentity = null;
            if (SelectedCompany is null)
            {
                Identities = null;
            }
            else
            {
                try
                {
                    Identities = new ObservableCollection<Identity>(Tools.Locator.IdentityECL.GetForCompany(SelectedCompany.Id));
                }
                catch (Exception ex)
                {
                    var op = reload ? Constants.Reload : Constants.Load;
                    PopupManager.Popup($"Failed to {op} Identities", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                    Environment.Exit(Constants.IdentitiesLoadFailed);
                }
            }
        }

        private void LoadAccounts(bool reload = false)
        {
            SelectedAccount = null;
            if (SelectedCompany is null)
            {
                Accounts = new ObservableCollection<Account>();
            }
            else
            {
                try
                {
                    Accounts = new ObservableCollection<Account>(Tools.Locator.AccountECL.GetForCompany(SelectedCompany.Id));
                }
                catch (Exception ex)
                {
                    var op = reload ? Constants.Reload : Constants.Load;
                    PopupManager.Popup($"Failed to {op} Accounts", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                    Environment.Exit(Constants.AccountsLoadFailed);
                }
                SelectedAccount = Accounts.Any() ? Accounts[0] : null;
                Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
            }
        }

        private void LoadTransactions(bool reload = false)
        {
            SelectedTransaction = null;
            if (SelectedAccount is null)
            {
                Transactions = null;
            }
            else
            {
                try
                {
                    Transactions = new ObservableCollection<Transaction>(Tools.Locator.TransactionECL.GetForAccount(SelectedAccount.Id));
                }
                catch (Exception ex)
                {
                    var op = reload ? Constants.Reload : Constants.Load;
                    PopupManager.Popup($"Failed to {op} Transactions", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                    Environment.Exit(Constants.TransactionsLoadFailed);
                }
                Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
            }
        }

        private static IEnumerable<int> GetOrphanedAccountNumbers()
        {
            var ret = new List<int>();
            var ecl = Tools.Locator.AccountECL;
            var ids = (from a in Tools.Locator.LedgerContext.AccountNumbers select a.AccountId).Distinct().ToList();
            foreach (var id in ids)
            {
                if (ecl.Read(id) == null)
                {
                    ret.Add(id);
                }
            }
            return ret;
        }

        private void AddTransaction(Transaction t)
        {
            var ix = 0;
            while (ix < Transactions.Count && Transactions[ix] > t)
            {
                ix++;
            }
            Transactions.Insert(ix, t);
            SelectedTransaction = t;
            SelectedTransaction = null;
        }

        private bool ValidateMoney(string value) => decimal.TryParse(value, out var _);

        private bool ValidateDate(DateTime? value) => value.HasValue && value.Value.Date <= DateTime.Now.Date;
    }
}
