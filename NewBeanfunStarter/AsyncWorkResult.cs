using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewBeanfunStarter
{
    public class AsyncWorkResult
    {
        public string Work { get; private set; }
        public bool Error { get; private set; }
        public object Argumment { get; private set; }

        public AsyncWorkResult(string Work, bool Error)
        {
            this.Work = Work;
            this.Error = Error;
            this.Argumment = null;
        }

        public AsyncWorkResult(string Work, bool Error, object Argument)
        {
            this.Work = Work;
            this.Error = Error;
            this.Argumment = Argument;
        }
    }
}
