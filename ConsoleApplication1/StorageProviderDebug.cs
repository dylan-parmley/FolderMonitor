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
    public class StorageProviderDebug 
    {
        CloudStorageAccount StorageAccount { get; set; }
        CloudBlobClient BlobClient { get; set; }
        CloudBlobContainer BlobContainer { get; set; }
       
        

        public bool DeleteFailed { get; set; }
        public bool AddBlobFailed { get; set; }

        public StorageProviderDebug()
        {
            
        }
        public void Initialize()
        {
            Console.WriteLine("");
            DeleteFailed = false;
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = StorageAccount.CreateCloudBlobClient();
            //TODO: Change to appropriate container name
            BlobContainer = BlobClient.GetContainerReference("mycontainer");
            BlobContainer.CreateIfNotExists();
        }
        public List<string> GetFileNames()
        {
            return null;
        }
        public void Create(Stream filestream, string filename) { }

        public void Create(string filepath, string filename)
        {
            Console.WriteLine("CreateBlob(string " + filepath + ", string " + filename + ")");
        }
        
        public void PrintFolder(string filePath)
        {
            Console.WriteLine("PrintBlobFolder()");
        }
        
        public void Delete(string fileName)
        {
            CloudBlockBlob blockBlob = BlobContainer.GetBlockBlobReference(fileName);
            if (FileExists(fileName))
            {
                blockBlob = BlobContainer.GetBlockBlobReference(fileName);
                blockBlob.Delete();
            }
            else
            {
                DeleteFailed = true;
            }

        }
        public bool FileExists(string fileName)
        {
            if (0 < BlobContainer.ListBlobs().OfType<CloudBlockBlob>().Where(e => e.Name.Equals(fileName)).Count())
                return true;
            else
                return false;
        }
        
        //public void PrintBlobFolder()
        //{
        //    // Loop over items within the container and output the length and URI.
        //    foreach (IListBlobItem item in BlobContainer.ListBlobs(null, false))
        //    {
        //        if (item.GetType() == typeof(CloudBlockBlob))
        //        {
        //            CloudBlockBlob blob = (CloudBlockBlob)item;

        //            Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

        //        }
        //        else if (item.GetType() == typeof(CloudPageBlob))
        //        {
        //            CloudPageBlob pageBlob = (CloudPageBlob)item;

        //            Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

        //        }
        //        else if (item.GetType() == typeof(CloudBlobDirectory))
        //        {
        //            CloudBlobDirectory directory = (CloudBlobDirectory)item;

        //            Console.WriteLine("Directory: {0}", directory.Uri);
        //        }
        //    }
        //}
    }
}
