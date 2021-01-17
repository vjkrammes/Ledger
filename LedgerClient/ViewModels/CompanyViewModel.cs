using LedgerClient.ECL.DTO;
using LedgerClient.Infrastructure;

using System;

namespace LedgerClient.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        #region Properties

        private string _savedName;

        private Company _company;
        public Company Company
        {
            get => _company;
            set
            {
                SetProperty(ref _company, value);
                _editing = Company.Id != 0;
                _savedName = Company.Name;
            }
        }

        private bool _editing;

        #endregion

        #region Command Methods

        public override bool OkCanExecute() => !string.IsNullOrEmpty(Company.Name);

        public override void OK()
        {
            if (_editing && Company.Name != _savedName && Tools.Locator.CompanyECL.Read(Company.Name) != null)
            {
                Duplicate();
                return;
            }
            else if (!_editing && Tools.Locator.CompanyECL.Read(Company.Name) != null)
            {
                Duplicate();
                return;
            }
            base.OK();
        }

        #endregion

        #region Events

        public event EventHandler FocusRequested;

        #endregion

        #region Utility Methods

        private void Duplicate()
        {
            PopupManager.Popup($"A Company with the name '{Company.Name}' already exists", "Duplicate Company", PopupButtons.Ok,
                PopupImage.Stop);
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public CompanyViewModel() => Company = new Company();
    }
}
