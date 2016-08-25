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

namespace ConsoleApplication1
{

    public class CloudStorageRepository : IStorageRepository
    {

        public CloudStorageAccount StorageAccount { get; set; }
        public CloudBlobClient BlobClient { get; set; }
        public CloudBlobContainer BlobContainer { get; set; }
        private static Logger _Logger;

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

            _Logger = LogManager.GetCurrentClassLogger();
            //TODO: Change to appropriate container name
            BlobContainer = BlobClient.GetContainerReference("mycontainer");
            //catch 400 


            //BlobContainer = BlobClient.GetContainerReference("pdflobs");
            BlobContainer.CreateIfNotExists();
        }

        //naming conventions? 
        public void CreateBlob(string filePath, string fileName)
        {
            string fullFilePath = Path.Combine(filePath, fileName);

            CloudBlockBlob blockBlob = BlobContainer.GetBlockBlobReference(fileName);


            if (!FileExists(fileName))
            {
                using (var fileStream = System.IO.File.OpenRead(fullFilePath))
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


        public void DeleteBlob(string fileName)
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
    }
}
