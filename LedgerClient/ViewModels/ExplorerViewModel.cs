using LedgerClient.Infrastructure;
using LedgerClient.Interfaces;
using LedgerClient.Models;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LedgerClient.ViewModels
{
    public class ExplorerViewModel : ViewModelBase
    {
        #region Properties

        private string _banner;
        public string Banner
        {
            get => _banner;
            set => SetProperty(ref _banner, value);
        }

        private bool _isFolderPicker;
        public bool IsFolderPicker
        {
            get => _isFolderPicker;
            set
            {
                SetProperty(ref _isFolderPicker, value);
                if (IsFolderPicker)
                {
                    Banner = "Select a Directory";
                }
                else
                {
                    Banner = "Select a File";
                }
            }
        }

        private ExplorerItem _rootItem;
        public ExplorerItem RootItem
        {
            get => _rootItem;
            set
            {
                SetProperty(ref _rootItem, value);
                if (RootItem is null)
                {
                    Root = new ReadOnlyCollection<ExplorerItem>(Array.Empty<ExplorerItem>());
                }
                else
                {
                    Root = new ReadOnlyCollection<ExplorerItem>(new ExplorerItem[] { RootItem });
                }
            }
        }

        private ReadOnlyCollection<ExplorerItem> _root;
        public ReadOnlyCollection<ExplorerItem> Root
        {
            get => _root;
            set => SetProperty(ref _root, value);
        }

        private ExplorerItem _selectedItem;
        public ExplorerItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private readonly IExplorerService _explorer;

        #endregion

        #region Commands

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

        #endregion

        #region Command Methods

        public override bool OkCanExecute()
        {
            if (SelectedItem is null)
            {
                return false;
            }
            return SelectedItem.Type switch
            {
                ExplorerItemType.Drive => true,
                ExplorerItemType.Directory => IsFolderPicker,
                ExplorerItemType.File => !IsFolderPicker,
                _ => false
            };
        }

        private void WindowLoaded()
        {
            RootItem = new ExplorerItem(!IsFolderPicker)
            {
                Name = "This Computer",
                Type = ExplorerItemType.ThisComputer
            };
            foreach (var item in ExplorerItem.Drives(_explorer.GetDrives(), !IsFolderPicker))
            {
                item.Children.Add(ExplorerItem.Placeholder);
                RootItem.Children.Add(item);
            }
            RootItem.IsExpanded = true;
        }

        #endregion

        public ExplorerViewModel(IExplorerService explorer) => _explorer = explorer;
    }
}
