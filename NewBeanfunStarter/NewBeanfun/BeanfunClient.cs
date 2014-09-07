using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NewBeanfun
{
    public class BeanfunClient
    {
        public bool StillAlive { get; private set; }
        private List<BeanfunGame> Games;
        public BeanfunClient(List<BeanfunGame> Games)
        {
            this.Games = new List<BeanfunGame>(Games);
        }

        public bool Initialize()
        {

        }

        public Image GetCaptchaImage()
        {

        }

        public bool Login(string Account, string Password, string CaptchaCode)
        {
            this.StillAlive = true;
        }

        public List<BeanfunGame> ListGames()
        {

        }

        public bool Ping()
        {
            this.StillAlive = true;
        }
    }
}
