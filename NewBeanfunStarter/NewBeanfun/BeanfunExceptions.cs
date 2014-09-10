using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewBeanfun
{
    public class BeanfunIsBusyException : Exception
    {
        public BeanfunIsBusyException() : base() { }
        public BeanfunIsBusyException(string message) : base(message) { }
    }
}
