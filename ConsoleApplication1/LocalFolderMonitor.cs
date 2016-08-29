using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;



namespace ConsoleApplication1
{
    public class LocalFolderMonitor : IFolderMonitor
    {
        //either the dropbox folder or local file
        public string FilePath { get; set; }
        public IStorageRepository CloudStorageProvider { get; set; }
        public FileSystemWatcher Watcher { get; set; }

        public  DropboxStorageRepository DropboxStorageProvider { get; set; }

        public LocalFolderMonitor()
        {

        }
        public LocalFolderMonitor(string folderName, IStorageRepository storageProvider)
        {
            Initialize(folderName, storageProvider);

        }
        public LocalFolderMonitor(string folderName, IStorageRepository cloudStorageProvider,DropboxStorageRepository dropboxStorageProvider )
        {
            Initialize(folderName, cloudStorageProvider, dropboxStorageProvider);
        }

        public void Initialize(string folderName, IStorageRepository cloudStorageProvider, DropboxStorageRepository dropboxStorageProvider)
        {
            FilePath = folderName;
            CloudStorageProvider = cloudStorageProvider;
            CloudStorageProvider.Initialize();

            DropboxStorageProvider = dropboxStorageProvider;
            
          
        }

        public void Initialize(string folderName, IStorageRepository storageProvider)
        {
            FilePath = folderName;
            CloudStorageProvider = storageProvider;
            CloudStorageProvider.Initialize();

            //Create a new FileSystemWatcher and set its properties.
            Watcher = new FileSystemWatcher();
            Watcher.Path = folderName;

            Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName; ;
            // Only watch text files.
            Watcher.Filter = "*.pdf";

            // Add event handlers.
            //Watcher.Changed += new FileSystemEventHandler(OnChanged);
            Watcher.Created += new FileSystemEventHandler(OnChanged);
            
            //Watcher.Error += new FileSystemEventHandler(OnChanged); 
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Observe()
        {
            // Begin watching.
            Watcher.EnableRaisingEvents = true;

            //Console.WriteLine("Press \'q\' to quit the sample.");
            //while (Console.Read() != 'q') ;
        }


        //create a blob of the file added to the folder we are watching in azure
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            CloudStorageProvider.Create(FilePath, e.Name);
        }

       


    }
}
