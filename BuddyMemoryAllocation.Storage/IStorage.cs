using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyMemoryAllocation.Storage
{
    public interface IStorage<T>
    {
        List<T> Read(string pathFile);
        void Write(IList<T> list, string pathFile);
    }
}
