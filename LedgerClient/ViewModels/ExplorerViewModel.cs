using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LedgerClient.Infrastructure;
using LedgerClient.Models;
using LedgerClient.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

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
                    Root = new ReadOnlyCollection<ExplorerItem>(new ExplorerItem[] { });
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

        #region Utility Methods

        private void ItemCollapsed(object sender, RoutedEventArgs e)
        {
            if (!(sender is ExplorerItem item))
            {
                return;
            }
            item.Children.Clear();
            item.Children.Add(ExplorerItem.Placeholder);
        }

        private void ItemExpanded(object sender, RoutedEventArgs e)
        {
            if (!(sender is ExplorerItem item))
            {
                return;
            }
            if (item.Children.Count == 1 || item.Children[0].Type == ExplorerItemType.Placeholder)
            {
                item.Children.Clear();
                switch (item.Type)
                {
                    case ExplorerItemType.ThisComputer:
                        foreach (var drive in ExplorerItem.Drives(_explorer.GetDrives()))
                        {
                            item.Children.Add(drive);
                        }
                        break;
                    case ExplorerItemType.Drive:
                    case ExplorerItemType.Directory:
                        foreach (var dir in ExplorerItem.Directories(_explorer.GetDirectories(item.Path)))
                        {
                            item.Children.Add(dir);
                        }
                        if (!IsFolderPicker)
                        {
                            foreach (var file in ExplorerItem.Files(_explorer.GetFiles(item.Path)))
                            {
                                item.Children.Add(file);
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        public ExplorerViewModel(IExplorerService explorer)
        {
            _explorer = explorer;
        }
    }
}
