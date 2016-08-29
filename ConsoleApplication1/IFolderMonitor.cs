using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication1
{
    public interface IFolderMonitor
    {
        //IFolderMonitor();
        //IFolderMonitor(string folderName, IStorageProvider storageProvider);
        string FilePath { get; set; }
        IStorageRepository CloudStorageProvider { get; set; }
        //FileSystemWatcher Watcher { get; set; }
        //bool Running { get; set; }

        //void Initialize(string folderName, IStorageRepository storageProvider);
        void Observe();
        //void OnChanged(object source, FileSystemEventArgs e);





    }
}
