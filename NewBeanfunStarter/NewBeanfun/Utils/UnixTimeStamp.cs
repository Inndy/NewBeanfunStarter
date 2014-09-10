using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewBeanfun.Utils
{
    public class UnixTimeStamp
    {
        public static long Now()
        {
            return DateTime.UtcNow.Ticks / 10000 - 62135596800000L;
        }
    }
}
