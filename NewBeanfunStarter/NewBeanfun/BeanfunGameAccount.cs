using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewBeanfun
{
    public class BeanfunGameAccount
    {
        public string Account { get; private set; }
        public string Number { get; private set; }
        public string Name { get; private set; }
        public BeanfunGame Game { get; private set; }

        public BeanfunGameAccount(string Account, string Number, string Name, BeanfunGame Game)
        {
            this.Account = Account;
            this.Number = Number;
            this.Name = Name;
            this.Game = Game;
        }
    }
}
