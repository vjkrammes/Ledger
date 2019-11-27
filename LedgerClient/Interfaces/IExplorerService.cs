using System.Collections.Generic;
using System.IO;

namespace LedgerClient.Interfaces
{
    public interface IExplorerService
    {
        IEnumerable<FileInfo> GetFiles(string path);
        IEnumerable<DirectoryInfo> GetDirectories(string path);
        IEnumerable<DriveInfo> GetDrives();
    }
}
