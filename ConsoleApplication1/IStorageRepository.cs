﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConsoleApplication1
{
    public interface IStorageRepository
    {
        //unsure if interface should include the properties
        //CloudStorageAccount StorageAccount { get; set; }
        //CloudBlobClient BlobClient { get; set; }
        //CloudBlobContainer BlobContainer { get; set; }
       
        void Initialize();
        void CreateBlob(string filepath, string filename);
        void DeleteBlob(string name);
    }
}
