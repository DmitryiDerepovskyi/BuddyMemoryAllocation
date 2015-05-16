using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BuddyMemoryAllocation.Storage
{
    public class StorageFileXml<T> : IStorage<T> where T : class 
    {
        public List<T> Read(string pathFile)
        {
            if(!File.Exists(pathFile)) throw new FileNotFoundException("Don't find file");
            var list = new List<T>();
            var deserializor = new XmlSerializer(typeof(List<T>));
            using (Stream fStream = new FileStream(pathFile,
               FileMode.Open, FileAccess.Read, FileShare.None))
            {
                list = (List<T>)deserializor.Deserialize(fStream);
            }
            return list;
        }

        public void Write(IList<T> list, string pathFile)
        {
            Type listType = typeof(IList<T>);
            var listT = list.GetType();
            var serializor = new XmlSerializer(listT);
            using (Stream fStream = new FileStream(pathFile,
                    FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializor.Serialize(fStream, list);
            }
        }
    }
}
