using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LedgerClient.Interfaces;

using LedgerLib.Infrastructure;

namespace LedgerClient.Infrastructure
{
    public class ExplorerService : IExplorerService
    {
        public IEnumerable<FileInfo> GetFiles(string path)
        {
            List<FileInfo> ret = new List<FileInfo>();
            try
            {
                var files = Directory.GetFiles(path, "*.*");
                foreach (var file in files)
                {
                    ret.Add(new FileInfo(file));
                }
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Error accessing path '{path}'", Constants.IOE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
            return ret;
        }

        public IEnumerable<DirectoryInfo> GetDirectories(string path)
        {
            List<DirectoryInfo> ret = new List<DirectoryInfo>();
            try
            {
                var directories = Directory.GetDirectories(path);
                foreach (var directory in directories)
                {
                    ret.Add(new DirectoryInfo(directory));
                }
            }
            catch (Exception ex)
            {
                PopupManager.Popup($"Error accessing path '{path}'", Constants.IOE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            }
            return ret;
        }

        public IEnumerable<DriveInfo> GetDrives() => DriveInfo.GetDrives().ToList();
    }
}
