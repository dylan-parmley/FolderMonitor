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
        private static Logger _Logger =LogManager.GetCurrentClassLogger(); 
        static void Main(string[] args)
        {
            FolderMonitor folderMonitor = new FolderMonitor("C://Users//dylan.parmley//Desktop//FindThis", new CloudStorageRepository());
            //folderMonitor.StorageProvider.PrintBlobFolder();

            //folderMonitor.StorageProvider.DeleteBlob("unitTestFile");
            //folderMonitor.StorageProvider.PrintBlobFolder();
            //folderMonitor.Observe();
           
            folderMonitor.StorageProvider.DeleteBlob("Grilled Cheese");
            //folderMonitor.StorageProvider.
            //createTestFile(".pdf");
            //deleteTestFile(".pdf");


        }

        public static void createTestFile(string fileType)
        {
            string pathString = "C://Users//dylan.parmley//Desktop//FindThis";
            string fileName = "unitTestFile" + fileType;

            pathString = Path.Combine(pathString, fileName);


            if (!File.Exists(pathString))
            {
                using (FileStream fs = System.IO.File.Create(pathString))
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
