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
            set
            {
                SetProperty(ref _isExpanded, value);
                if (Children != null)
                {
                    if (IsExpanded)
                    {
                        LoadChildren();
                    }
                    else
                    {
                        Children.Clear();
                        if (Type == ExplorerItemType.Directory || Type == ExplorerItemType.Drive)
                        {
                            Children.Add(Placeholder);
                        }
                    }
                }
            }
        }

        private IList<ExplorerItem> _children;
        public IList<ExplorerItem> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        private readonly bool _includeFiles = true;

        #endregion

        #region Methods

        public override string ToString() => Name;

        private void LoadChildren()
        {
            if (Children != null)
            {
                Children.Clear();
                switch (Type)
                {
                    case ExplorerItemType.Drive:
                    case ExplorerItemType.Directory:
                        foreach (var dir in Directories(Tools.Locator.ExplorerService.GetDirectories(Path), _includeFiles))
                        {
                            dir.Children.Add(Placeholder);
                            Children.Add(dir);
                        }
                        if (_includeFiles)
                        {
                            foreach (var file in Files(Tools.Locator.ExplorerService.GetFiles(Path), _includeFiles))
                            {
                                file.Children.Add(Placeholder);
                                Children.Add(file);
                            }
                        }
                        break;
                    case ExplorerItemType.ThisComputer:
                        foreach (var drive in Drives(Tools.Locator.ExplorerService.GetDrives(), _includeFiles))
                        {
                            drive.Children.Add(Placeholder);
                            Children.Add(drive);
                        }
                        break;
                }
            }
        }

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
            Children = new List<ExplorerItem>();
            _includeFiles = true;
        }

        public ExplorerItem(bool includefiles) : this() => _includeFiles = includefiles;

        public ExplorerItem(string directory) : this() => _name = directory;

        public ExplorerItem(string directory, bool includefiles) : this(includefiles) => _name = directory;

        public ExplorerItem(DirectoryInfo dirinfo) : this()
        {
            Type = ExplorerItemType.Directory;
            Name = dirinfo.Name;
            Path = dirinfo.FullName;
            Root = dirinfo.Root.Name;
        }

        public ExplorerItem(DirectoryInfo dirinfo, bool includefiles) : this(includefiles)
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

        public ExplorerItem(FileInfo fileinfo, bool includefiles) : this(includefiles)
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

        public ExplorerItem(DriveInfo driveinfo, bool includefiles) : this(includefiles)
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

        public static IEnumerable<ExplorerItem> Directories(IEnumerable<DirectoryInfo> directories, bool includefiles = true)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var directory in directories)
            {
                ret.Add(new ExplorerItem(directory, includefiles));
            }
            return ret;
        }

        public static IEnumerable<ExplorerItem> Files(IEnumerable<FileInfo> files, bool includefiles = true)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var file in files)
            {
                ret.Add(new ExplorerItem(file, includefiles));
            }
            return ret;
        }

        public static IEnumerable<ExplorerItem> Drives(IEnumerable<DriveInfo> drives, bool includefiles = true)
        {
            List<ExplorerItem> ret = new List<ExplorerItem>();
            foreach (var drive in drives)
            {
                ret.Add(new ExplorerItem(drive, includefiles));
            }
            return ret;
        }

        #endregion
    }
}
