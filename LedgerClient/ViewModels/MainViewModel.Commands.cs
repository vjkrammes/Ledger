using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using LedgerClient.Infrastructure;

namespace LedgerClient.ViewModels
{
    public partial class MainViewModel
    {

        private RelayCommand _manageAccountTypesCommand;
        public ICommand ManageAccountTypesCommand
        {
            get
            {
                if (_manageAccountTypesCommand is null)
                {
                    _manageAccountTypesCommand = new RelayCommand(parm => ManageAccountTypesClick(), parm => AlwaysCanExecute());
                }
                return _manageAccountTypesCommand;
            }
        }

        private RelayCommand _managePoolsCommand;
        public ICommand ManagePoolsCommand
        {
            get
            {
                if (_managePoolsCommand is null)
                {
                    _managePoolsCommand = new RelayCommand(parm => ManagePoolsClick(), parm => AlwaysCanExecute());
                }
                return _managePoolsCommand;
            }
        }

        private RelayCommand _cleanUpOrphanedAccountNumbersCommand;
        public ICommand CleanUpOrphanedAccountNumbersCommand
        {
            get
            {
                if (_cleanUpOrphanedAccountNumbersCommand is null)
                {
                    _cleanUpOrphanedAccountNumbersCommand = new RelayCommand(parm => CleanUpOrphanedAccountNumbersClick(),
                        parm => CleanUpOrphanedAccountNumbersCanClick());
                }
                return _cleanUpOrphanedAccountNumbersCommand;
            }
        }

        private RelayCommand _addCompanyCommand;
        public ICommand AddCompanyCommand
        {
            get
            {
                if (_addCompanyCommand is null)
                {
                    _addCompanyCommand = new RelayCommand(parm => AddCompanyClick(), parm => AlwaysCanExecute());
                }
                return _addCompanyCommand;
            }
        }

        private RelayCommand _editCompanyCommand;
        public ICommand EditCompanyCommand
        {
            get
            {
                if (_editCompanyCommand is null)
                {
                    _editCompanyCommand = new RelayCommand(parm => EditCompanyClick(), parm => CompanySelected());
                }
                return _editCompanyCommand;
            }
        }

        private RelayCommand _deleteCompanyCommand;
        public ICommand DeleteCompanyCommand
        {
            get
            {
                if (_deleteCompanyCommand is null)
                {
                    _deleteCompanyCommand = new RelayCommand(parm => DeleteCompanyClick(), parm => DeleteCompanyCanClick());
                }
                return _deleteCompanyCommand;
            }
        }

        private RelayCommand _addAccountCommand;
        public ICommand AddAccountCommand
        {
            get
            {
                if (_addAccountCommand is null)
                {
                    _addAccountCommand = new RelayCommand(parm => AddAccountClick(), parm => CompanySelected());
                }
                return _addAccountCommand;
            }
        }

        private RelayCommand _editAccountCommand;
        public ICommand EditAccountCommand
        {
            get
            {
                if (_editAccountCommand is null)
                {
                    _editAccountCommand = new RelayCommand(parm => EditAccountClick(), parm => AccountSelected());
                }
                return _editAccountCommand;
            }
        }

        private RelayCommand _viewHistoryCommand;
        public ICommand ViewHistoryCommand
        {
            get
            {
                if (_viewHistoryCommand is null)
                {
                    _viewHistoryCommand = new RelayCommand(parm => ViewHistoryClick(), parm => ViewHistoryCanClick());
                }
                return _viewHistoryCommand;
            }
        }

        private RelayCommand _deleteAccountCommand;
        public ICommand DeleteAccountCommand
        {
            get
            {
                if (_deleteAccountCommand is null)
                {
                    _deleteAccountCommand = new RelayCommand(parm => DeleteAccountClick(), parm => DeleteAccountCanClick());
                }
                return _deleteAccountCommand;
            }
        }

        private RelayCommand _copyURLCommand;
        public ICommand CopyURLCommand
        {
            get
            {
                if (_copyURLCommand is null)
                {
                    _copyURLCommand = new RelayCommand(parm => CopyURLClick(), parm => IdentitySelected());
                }
                return _copyURLCommand;
            }
        }

        private RelayCommand _copyPasswordCommand;
        public ICommand CopyPasswordCommand
        {
            get
            {
                if (_copyPasswordCommand is null)
                {
                    _copyPasswordCommand = new RelayCommand(parm => CopyPasswordClick(), parm => CopyPasswordCanClick());
                }
                return _copyPasswordCommand;
            }
        }

        private RelayCommand _clearClipboardCommand;
        public ICommand ClearClipboardCommand
        {
            get
            {
                if (_clearClipboardCommand is null)
                {
                    _clearClipboardCommand = new RelayCommand(parm => ClearClipboardClick(), parm => AlwaysCanExecute());
                }
                return _clearClipboardCommand;
            }
        }

        private RelayCommand _addIdentityCommand;
        public ICommand AddIdentityCommand
        {
            get
            {
                if (_addIdentityCommand is null)
                {
                    _addIdentityCommand = new RelayCommand(parm => AddIdentityClick(), parm => CompanySelected());
                }
                return _addIdentityCommand;
            }
        }

        private RelayCommand _editIdentityCommand;
        public ICommand EditIdentityCommand
        {
            get
            {
                if (_editIdentityCommand is null)
                {
                    _editIdentityCommand = new RelayCommand(parm => EditIdentityClick(), parm => IdentitySelected());
                }
                return _editIdentityCommand;
            }
        }

        private RelayCommand _deselectIdentityCommand;
        public ICommand DeselectIdentityCommand
        {
            get
            {
                if (_deselectIdentityCommand is null)
                {
                    _deselectIdentityCommand = new RelayCommand(parm => DeselectIdentityClick(), parm => IdentitySelected());
                }
                return _deselectIdentityCommand;
            }
        }

        private RelayCommand _deleteIdentityCommand;
        public ICommand DeleteIdentityCommand
        {
            get
            {
                if (_deleteIdentityCommand is null)
                {
                    _deleteIdentityCommand = new RelayCommand(parm => DeleteIdentityClick(), parm => IdentitySelected());
                }
                return _deleteIdentityCommand;
            }
        }

        private RelayCommand _importCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (_importCommand is null)
                {
                    _importCommand = new RelayCommand(parm => ImportClick(), parm => ImportCanClick());
                }
                return _importCommand;
            }
        }

        private RelayCommand _iconHeightCommand;
        public ICommand IconHeightCommand
        {
            get
            {
                if (_iconHeightCommand is null)
                {
                    _iconHeightCommand = new RelayCommand(parm => IconHeight(parm), parm => AlwaysCanExecute());
                }
                return _iconHeightCommand;
            }
        }

        private RelayCommand _toggleStatusbarCommand;
        public ICommand ToggleStatusbarCommand
        {
            get
            {
                if (_toggleStatusbarCommand is null)
                {
                    _toggleStatusbarCommand = new RelayCommand(parm => ToggleStatusbar(), parm => AlwaysCanExecute());
                }
                return _toggleStatusbarCommand;
            }
        }

        private RelayCommand _backupCommand;
        public ICommand BackupCommand
        {
            get
            {
                if (_backupCommand is null)
                {
                    _backupCommand = new RelayCommand(parm => BackupClick(), parm => AlwaysCanExecute());
                }
                return _backupCommand;
            }
        }

        private RelayCommand _palletteCommand;
        public ICommand PalletteCommand
        {
            get
            {
                if (_palletteCommand is null)
                {
                    _palletteCommand = new RelayCommand(parm => PalletteClick(), parm => AlwaysCanExecute());
                }
                return _palletteCommand;
            }
        }

        private RelayCommand _aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                if (_aboutCommand is null)
                {
                    _aboutCommand = new RelayCommand(parm => AboutClick(), parm => AlwaysCanExecute());
                }
                return _aboutCommand;
            }
        }

        private RelayCommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand
        {
            get
            {
                if (_windowLoadedCommand is null)
                {
                    _windowLoadedCommand = new RelayCommand(parm => WindowLoaded(), parm => AlwaysCanExecute());
                }
                return _windowLoadedCommand;
            }
        }
    }
}
