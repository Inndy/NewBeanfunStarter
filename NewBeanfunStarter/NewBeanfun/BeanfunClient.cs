using NewBeanfun.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace NewBeanfun
{
    public class BeanfunClient
    {
        public bool StillAlive { get; private set; }
        private List<BeanfunGame> Games;
        private CustomWebClient webClient;
        private CookieContainer cookieContainer;
        private Dictionary<string, string> sessionData;
        private string captchaId;
        public BeanfunClient(List<BeanfunGame> games)
        {
            this.Games = new List<BeanfunGame>(games);
        }

        public bool Initialize()
        {
            string userAgent = UserAgentGenerator.Random();
            this.cookieContainer = new CookieContainer();
            this.webClient = new CustomWebClient(this.cookieContainer);
            this.webClient.Headers.Add(HttpRequestHeader.UserAgent, userAgent);
            this.sessionData = new Dictionary<string, string>();

            this.webClient.DownloadData("https://tw.new.beanfun.com/beanfun_block/bflogin/default.aspx?service=999999_T0");
            string retUrl = this.webClient.ResponseUri.ToString();
            string skey = FastRegex.Extract(@"checkin_step2\.aspx\?skey=(\w{20})", retUrl);
            if (skey == null)
                return false;


            this.sessionData["skey"] = skey;
            string loginUrl = "https://tw.newlogin.beanfun.com/login/id-pass_form.aspx?skey=" + this.sessionData["skey"];
            string content = this.webClient.DownloadUtf8String(loginUrl);
            this.captchaId = FastRegex.Extract(@"samplecaptcha\x22 value=\x22(\w{32})\x22", content);
            this.sessionData["VIEWSTATE"] = FastRegex.Extract(@"\x22__VIEWSTATE\x22 value=\x22([^\x22]+)\x22", content);
            this.sessionData["EVENTVALIDATION"] = FastRegex.Extract(@"\x22__EVENTVALIDATION\x22 value=\x22([^\x22]+)\x22", content);

            return captchaId != null && this.sessionData["VIEWSTATE"] != null && this.sessionData["EVENTVALIDATION"] != null;
        }

        public Image GetCaptchaImage()
        {
            return null;
        }

        public bool Login(string account, string password, string captchaCode)
        {
            this.StillAlive = true;
            return false;
        }

        public List<BeanfunGame> ListGames()
        {
            return null;
        }

        public bool Ping()
        {
            this.StillAlive = true;
            return false;
        }
    }
}
