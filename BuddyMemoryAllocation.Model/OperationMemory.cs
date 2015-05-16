using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyMemoryAllocation.Model
{
    public class OperationMemory
    {
        public OperationMemory(int size, int minimalBlockSize, Units units)
        {
            Size = size;
            MinimalBlockSize = minimalBlockSize;
            Units = units;
            Map = new LinkedList<BlockMemory>();
            Map.AddFirst(new BlockMemory(0, size, null));
        }
        public int Size { get; set; }
        public int BusyMemory { get; set; }
        public int FreeMemory
        {
            get { return Size - BusyMemory; }
        }

        public int MinimalBlockSize { get; set; }
        public Units Units { get; set; }
        public LinkedList<BlockMemory> Map { get; set; }
    }
}
