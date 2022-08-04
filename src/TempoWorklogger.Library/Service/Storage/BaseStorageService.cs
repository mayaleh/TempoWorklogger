using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoWorklogger.Library.Service.Storage
{
    public abstract class BaseStorageService
    {
        private const string StorageDir = "storage";

        protected string EnsureFileExist(string filename, string defaultContent = "[]")
        {
            string storageDir = EnsureDirExists(StorageDir);

            var filePath = Path.Combine(storageDir, filename); ;

            if (File.Exists(filePath) == false)
            {
                File.WriteAllText(filePath, defaultContent);
            }

            return filePath;
        }

        private static string EnsureDirExists(string dirName)
        {
            var storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirName);

            if (Directory.Exists(storagePath) == false)
            {
                Directory.CreateDirectory(storagePath);
            }

            return storagePath;
        }
    }
}
