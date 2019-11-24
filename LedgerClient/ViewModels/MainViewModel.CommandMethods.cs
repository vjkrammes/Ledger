using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;
using LedgerClient.Views;
using LedgerLib;
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

        private void ManageAccountTypesClick()
        {
            AccountTypeViewModel vm = Tools.Locator.AccountTypeViewModel;
            DialogSupport.ShowDialog<AccountTypeWindow>(vm, Application.Current.MainWindow);
            Tools.Locator.StatusbarViewModel.Update(SelectedAccount);
        }

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

        private bool ImportCanClick()
        {
            var context = Tools.Locator.LedgerContext;
            var ret = context.DatabaseExists(Constants.OldDatabase); // DatabaseExists return bool?
            return ret == true;
        }

        private void ImportClick()
        {

        }

        private void IconHeight(object parm)
        {
            if (!(parm is string h) || !double.TryParse(h, out double height))
            {
                return;
            }
            Tools.Locator.Settings.IconHeight = height;
            Application.Current.Resources[Constants.IconHeight] = height;
        }

        private void ToggleStatusbar()
        {
            Tools.Locator.Settings.ShowStatusbar = !Tools.Locator.Settings.ShowStatusbar;
            StatusbarVisibility = Tools.Locator.Settings.ShowStatusbar ? Visibility.Visible : Visibility.Collapsed;
            Tools.Locator.StatusbarViewModel.StatusbarVisibility = StatusbarVisibility;
        }

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
        }
    }
}
