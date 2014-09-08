using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NewBeanfun.Utils
{
    public static class FastRegex
    {
        public static string Extract(string regexp, string data)
        {
            Regex reg = new Regex(regexp);
            if (reg.IsMatch(data))
            {
                return reg.Match(data).Groups[1].Value;
            }
            else
            {
                return null;
            }
        }
    }
}
