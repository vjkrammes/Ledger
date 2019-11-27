using System.Collections.Generic;
using System.IO;
using System.Linq;

using LedgerClient.Interfaces;

namespace LedgerClient.Infrastructure
{
    public class ExplorerService : IExplorerService
    {
        public IEnumerable<FileInfo> GetFiles(string path)
        {
            List<FileInfo> ret = new List<FileInfo>();
            var files = Directory.GetFiles(path, "*.*");
            foreach (var file in files)
            {
                ret.Add(new FileInfo(file));
            }
            return ret;
        }

        public IEnumerable<DirectoryInfo> GetDirectories(string path)
        {
            List<DirectoryInfo> ret = new List<DirectoryInfo>();
            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                ret.Add(new DirectoryInfo(directory));
            }
            return ret;
        }

        public IEnumerable<DriveInfo> GetDrives() => DriveInfo.GetDrives().ToList();
    }
}
