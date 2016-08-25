﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;


namespace ConsoleApplication1
{
    public class FolderMonitorDebug
    {
        public string FolderName { get; set; }
        public IStorageRepository StorageProvider { get; set; }
        public FileSystemWatcher Watcher { get; set; }
        public bool Running { get; set; }

        public FolderMonitorDebug(string folderName, IStorageRepository storageProvider)
        {
            Initialize(folderName, storageProvider);
        }

        public void Initialize(string folderName, IStorageRepository storageProvider)
        {
            Running = false;

            FolderName = folderName;
            StorageProvider = storageProvider;

            // Create a new FileSystemWatcher and set its properties.
            Watcher = new FileSystemWatcher();
            Watcher.Path = folderName;

            Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName; ;
            // Only watch text files.
            Watcher.Filter = "*.pdf";

            // Add event handlers.
            //Watcher.Changed += new FileSystemEventHandler(OnChanged);
            Watcher.Created += new FileSystemEventHandler(OnChanged);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Observe()
        {

            // Begin watching.
            Watcher.EnableRaisingEvents = true;
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Running = true;
            StorageProvider.CreateBlob(FolderName, e.Name);

        }
    }
}
