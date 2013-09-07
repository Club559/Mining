using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer
{
    public class ServerStatus
    {
        public static double Usage = 0.2;

        public static double GetUsage()
        {
            return Usage;
        }
    }
}
