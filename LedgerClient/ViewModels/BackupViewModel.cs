using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using LedgerClient.Infrastructure;
using LedgerLib.Interfaces;
using LedgerLib;
using LedgerLib.Infrastructure;
using LedgerClient.Views;
using Microsoft.Extensions.Configuration;

namespace LedgerClient.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        #region Properties

        private string _filename;
        public string Filename
        {
            get => _filename;
            set => SetProperty(ref _filename, value);
        }

        private string _directory;
        public string Directory
        {
            get => _directory;
            set => SetProperty(ref _directory, value);
        }

        private ObservableCollection<FileInfo> _files;
        public ObservableCollection<FileInfo> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }

        private FileInfo _selectedFile;
        public FileInfo SelectedFile
        {
            get => _selectedFile;
            set => SetProperty(ref _selectedFile, value);
        }

        private readonly ISettingsService _settings;
        private readonly LedgerContext _context;
        private readonly IConfiguration _config;

        #endregion

        #region Commands

        private RelayCommand _changeCommand;
        public ICommand ChangeCommand
        {
            get
            {
                if (_changeCommand is null)
                {
                    _changeCommand = new RelayCommand(parm => ChangeClick(), parm => AlwaysCanExecute());
                }
                return _changeCommand;
            }
        }

        private RelayCommand _backupCommand;
        public ICommand BackupCommand
        {
            get
            {
                if (_backupCommand is null)
                {
                    _backupCommand = new RelayCommand(parm => BackupClick(), parm => BackupCanClick());
                }
                return _backupCommand;
            }
        }

        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand is null)
                {
                    _deleteCommand = new RelayCommand(parm => DeleteClick(), parm => FileSelected());
                }
                return _deleteCommand;
            }
        }

        #endregion

        #region Command Methods

        private void ChangeClick()
        {
            var vm = Tools.Locator.ExplorerViewModel;
            vm.IsFolderPicker = true;
            if (DialogSupport.ShowDialog<ExplorerWindow>(vm) != true)
            {
                return;
            }
            Directory = vm.SelectedItem.Path;
            _settings.BackupDirectory = Directory;
            Filename = Directory + @"\" + _config[Constants.DatabaseConfig];
            LoadFiles();
        }

        private bool BackupCanClick() => !string.IsNullOrEmpty(Filename);

        private void BackupClick()
        {
            if (string.IsNullOrEmpty(Filename))
            {
                return;
            }
            try
            {
                _context.Backup(Filename);
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Database backup failed", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                return;
            }
            PopupManager.Popup("Database backup complete", "Backup Complete", PopupButtons.Ok, PopupImage.Information);
            LoadFiles();
        }

        private bool FileSelected() => SelectedFile != null;

        private void DeleteClick()
        {
            if (SelectedFile is null)
            {
                return;
            }
            string msg = $"Delete backup file '{SelectedFile.FullName}'? Action cannot be undone.";
            if (PopupManager.Popup("Delete Backup File?", "Delete File?", msg, PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
            {
                SelectedFile = null;
                return;
            }
            try
            {
                File.Delete(SelectedFile.FullName);
            }
            catch (Exception ex)
            {
                PopupManager.Popup("Delete of file failed", Constants.IOE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
                SelectedFile = null;
                return;
            }
            LoadFiles();
        }

        #endregion

        #region Utility Methods

        private void LoadFiles()
        {
            string[] files = System.IO.Directory.GetFiles(Directory, "*.backup");
            Files = new ObservableCollection<FileInfo>();
            foreach (var file in files)
            {
                Files.Add(new FileInfo(file));
            }
        }

        #endregion

        public BackupViewModel(ISettingsService settings, LedgerContext context, IConfiguration config)
        {
            _settings = settings;
            _context = context;
            _config = config;
            string bd = _settings.BackupDirectory;
            if (string.IsNullOrEmpty(bd))
            {
                Filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _settings.BackupDirectory = Filename;
            }
            else
            {
                Filename = bd;
            }
            if (!Filename.EndsWith(@"\"))
            {
                Filename += @"\";
            }
            Filename += Constants.ProductName + ".backup";
            Directory = Path.GetDirectoryName(Filename);
            LoadFiles();
        }
    }
}
