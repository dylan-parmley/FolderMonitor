using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
namespace ConsoleApplication1
{
    public interface IStorageRepository
    {
        //unsure if interface should include the properties
        //CloudStorageAccount StorageAccount { get; set; }
        //CloudBlobClient BlobClient { get; set; }
        //CloudBlobContainer BlobContainer { get; set; }
       
        //insert
        void Create(string filepath, string filename);
        void Create(Stream data, string filepath);
        //delete
        void Delete(string name);
        void Initialize();
        List<string> GetFileNames();
        Task<List<string>> GetFileNames(string fileName);
    }
}
