using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;




namespace ConsoleApplication1
{
    class ProgramStart
    {
        private static Logger _Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            //FolderMonitor folderMonitor = new FolderMonitor("/FindFolderMonitor/FridayFriday.pdf", new CloudStorageRepository(), new DropboxStorageRepository());
            //string source ="/FindFolderMonitor/testPDF8.pdf";
            //string destination ="C://Users//dylan.parmley//Desktop//FindThis//testPDF8.pdf";
            //LocalFolderMonitor a = new LocalFolderMonitor("C://Users//dylan.parmley//Desktop//FindThis//", new CloudStorageRepository());
            OnlineFolderMonitor b = new OnlineFolderMonitor( "/FindFolderMonitor", new CloudStorageRepository(), new DropboxStorageRepository("UE9mv77GK0AAAAAAAAAAC-chQ1ilK5LFJu1E0VphQgEggIMpGx-VKKS_gvLl0Eje"));
            //a.CloudStorageProvider.Initialize();
            

            //folderMonitor.Initialize(folderName, cloudStorageProvider, dropboxStorageProvider);
            Task theTask = b.Observes();
            theTask.Wait();

          


        }
        
       
        public static void createTestFile(string fileType)
        {
            string pathString = "C://Users//dylan.parmley//Desktop//FindThis";
            string fileName = "unitTestFile" + fileType;

            pathString = Path.Combine(pathString, fileName);


            if (!File.Exists(pathString))
            {
                using (FileStream fs = File.Create(pathString))
                {

                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileName);
                return;
            }

        }

        public static void deleteTestFile(string fileType)
        {
            string pathString = "C://Users//dylan.parmley//Desktop//FindThis";
            string fileName = "unitTestFile" + fileType;
            pathString = Path.Combine(pathString, fileName);


            FileInfo fi = new FileInfo(pathString);
            try
            {
                fi.Delete();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
