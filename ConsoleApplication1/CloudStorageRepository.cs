using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Security.Permissions;
using System.IO;
using NLog;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApplication1
{

    public class CloudStorageRepository : IStorageRepository
    {

        public CloudStorageAccount StorageAccount { get; set; }
        public CloudBlobClient BlobClient { get; set; }
        public CloudBlobContainer BlobContainer { get; set; }
        private static Logger _Logger = LogManager.GetCurrentClassLogger();
        
        public CloudStorageRepository()
        {
            
        }

        public CloudStorageRepository(String ContainerReference)
        {

        }


        public void Initialize()
        {
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = StorageAccount.CreateCloudBlobClient();

            
            //TODO: Change to appropriate container name
            BlobContainer = BlobClient.GetContainerReference("mycontainer");
            //catch 400 

            PrintFolder();
            //BlobContainer = BlobClient.GetContainerReference("pdflobs");
            try { 
                    BlobContainer.CreateIfNotExists();
                }
            catch(Microsoft.WindowsAzure.Storage.StorageException e)
            {
                _Logger.Error("A connection was not established to the Azure Service at {0}", BlobContainer.StorageUri);
            }
        }

        public void Create(Stream file, string fileName)
        {
            CloudBlockBlob blockBlob = BlobContainer.GetBlockBlobReference(fileName);
            if (!FileExists(fileName))
            {
                using (var fileStream = file)
                {
                    blockBlob.UploadFromStream(fileStream);
                }
            }
        }

        //naming conventions? 
        public void Create(string filePath, string fileName)
        {
            string fullFilePath = Path.Combine(filePath, fileName);

            CloudBlockBlob blockBlob = BlobContainer.GetBlockBlobReference(fileName);


            if (!FileExists(fileName))
            {
                using (var fileStream = File.OpenRead(fullFilePath))
                {
                    blockBlob.UploadFromStream(fileStream);
                }
            }
            else
            {
                //file already exists on cloud
                _Logger.Error("Attempted to add file: {0} to {1} when it already exists", fileName, BlobContainer.Uri);
                throw (new Exception("File: "+fileName +" already exists in the container: "+BlobContainer.Name));
            }

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
                //file does not exist in container
                _Logger.Error("Attempted to delete file: {0} from {1} when it did not exist", fileName, BlobContainer.Uri);
                
                throw (new System.IO.FileNotFoundException("File: " + fileName + " does not exist in the directory " + BlobContainer.Uri));
            }


        }

        public bool FileExists(string fileName)
        {
            int countOfFileNames = BlobContainer.ListBlobs().OfType<CloudBlockBlob>().Where(e => e.Name.Equals(fileName)).Count();
            return countOfFileNames >0;
        }
        public void PrintFolder()
        {
            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in BlobContainer.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
        }
        public List<string> GetFileNames()
        {
            var cloudBlobs = BlobContainer.ListBlobs().OfType<CloudBlockBlob>();
            List<string> names = new List<string>();
            foreach (CloudBlockBlob cloud in cloudBlobs)
            {
                names.Add(cloud.Name);
            }
            return names;
        }
        public Task<List<string>> GetFileNames(string fileName) { return null; }
    }
}
