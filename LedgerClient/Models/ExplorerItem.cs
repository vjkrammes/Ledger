using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using LedgerClient.Infrastructure;

namespace LedgerClient.Models
{
    public class ExplorerItem : NotifyBase
    {

        #region Properties

        private ExplorerItemType _type;
        public ExplorerItemType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _path;
        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private string _extension;
        public string Extension
        {
            get => _extension;
            set => SetProperty(ref _extension, value);
        }

        private string _root;
        public string Root
        {
            get => _root;
            set => SetProperty(ref _root, value);
        }

        private long _size;
        public long Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private IList<ExplorerItem> _children;
        public IList<ExplorerItem> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        #endregion

        #region Methods

        public override string ToString() => Name;

        #endregion

        #region Constructors

        public ExplorerItem()
        {
            Type = ExplorerItemType.Unspecified;
            Name = string.Empty;
            Path = string.Empty;
            Extension = string.Empty;
            Root = string.Empty;
            Size = 0L;
            IsSelected = false;
            IsExpanded = false;
            Children = new ObservableCollection<ExplorerItem>();
        }

        public ExplorerItem(string directory) : this() => _name = directory;

        public ExplorerItem(DirectoryInfo dirinfo) : this()
        {
            Type = ExplorerItemType.Directory;
            Name = dirinfo.Name;
            Path = dirinfo.FullName;
            Root = dirinfo.Root.Name;
        }

        public ExplorerItem(FileInfo fileinfo) : this()
        {
            Type = ExplorerItemType.File;
            Name = fileinfo.Name;
            Path = fileinfo.FullName;
            Extension = fileinfo.Extension;
            Size = fileinfo.Length;
        }

        public ExplorerItem(DriveInfo driveinfo) : this()
        {
            Type = ExplorerItemType.Drive;
            if (driveinfo.Name.EndsWith("\\"))
            {
                Name = driveinfo.Name[..^1];
            }
            else
            {
                Name = driveinfo.Name;
            }
            Path = driveinfo.Name;
        }

        #endregion

        #region Factory Methods

        public static ExplorerItem Placeholder { get => new ExplorerItem { Type = ExplorerItemType.Placeholder }; }

        public static IEnumerable<ExplorerItem> Directories(IEnumerable<DirectoryInfo> directories)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var directory in directories)
            {
                ret.Add(new ExplorerItem(directory));
            }
            return ret;
        }

        public static IEnumerable<ExplorerItem> Files(IEnumerable<FileInfo> files)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var file in files)
            {
                ret.Add(new ExplorerItem(file));
            }
            return ret;
        }

        public static IEnumerable<ExplorerItem> Drives(IEnumerable<DriveInfo> drives)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var drive in drives)
            {
                ret.Add(new ExplorerItem(drive));
            }
            return ret;
        }

        #endregion
    }
}
