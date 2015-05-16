using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BuddyMemoryAllocation.Model
{
    [Serializable]
    public class Process
    {
        public Process()
        {
            Id = IdProcess.GetProcessId();
        }
        public Process(string name, int size)
        {
            Id = IdProcess.GetProcessId(); 
            Name = name;
            Size = size;
        }
        [XmlIgnore]
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Size { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}\nName: {1}\nSize: {2}\n",Id,Name,Size);
        }
    }
}
