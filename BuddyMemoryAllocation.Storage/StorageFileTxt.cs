using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyMemoryAllocation.Storage
{
    class StorageFileTxt<T> : IStorage<T> where T : class
    {
        public List<T> Read(string pathFile)
        {
            if(!File.Exists(pathFile)) throw new FileNotFoundException("pathFile");
            var list = new List<T>();
            File.ReadAllText(pathFile);
            return list;
        }

        public void Write(IList<T> list, string pathFile)
        {
            if(list == null) throw new ArgumentNullException("list");
        }
    }
}
