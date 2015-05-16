using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyMemoryAllocation.Model
{
    public class BlockMemory
    {
        public BlockMemory()
        {
            IsFree = true;
        }

        public BlockMemory(int address, int size, Process process)
        {
            Address = address;
            Size = size;
            Process = process;
            if (process == null) IsFree = true;
        }
        public int Address { get; private set; }
        public int Size { get; private set; }
        public Process Process { get; set; }
        public bool IsFree { get; private set; }

        public void Free()
        {
            Process = null;
            IsFree = true;
        }

        public void LoadProcess(Process process)
        {
            if(process == null) throw new ArgumentNullException("process");
            if(Size < process.Size) throw new ArgumentException("Size of process is greater than size of block");
            Process = process;
            IsFree = false;
        }

    }
}
