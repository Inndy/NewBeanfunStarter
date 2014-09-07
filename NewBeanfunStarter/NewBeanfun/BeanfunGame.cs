using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewBeanfun
{
    public class BeanfunGame
    {
        public string Name { get; private set; }
        public string ServiceCode { get; private set; }
        public string ServiceRegion { get; private set; }
        public string PPPPP { get; private set; }

        public BeanfunGame(string Name, string ServiceCode, string ServiceRegion, string PPPPP = null)
        {
            this.Name = Name;
            this.ServiceCode = ServiceCode;
            this.ServiceRegion = ServiceRegion;
            this.PPPPP = PPPPP;
        }
    }
}
