using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyMemoryAllocation.Model
{
    public static class IdProcess
    {
        private static int _id = -1;

        public static int GetProcessId()
        {
            _id++;
            return _id;
        }
    }
}
