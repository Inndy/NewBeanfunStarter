using NewBeanfun.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

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
        private Mutex mutex;
        public BeanfunClient(List<BeanfunGame> games)
        {
            this.Games = new List<BeanfunGame>(games);
            this.mutex = new Mutex();
        }

        public bool Initialize()
        {
            bool result = false;

            if (!this.mutex.WaitOne(0, false))
            {
                throw new BeanfunIsBusyException();
            }

            try
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

                result = captchaId != null && this.sessionData["VIEWSTATE"] != null && this.sessionData["EVENTVALIDATION"] != null;
            }
            catch (Exception)
            {
                this.mutex.ReleaseMutex();
                return false;
            }

            this.mutex.ReleaseMutex();
            return result;
        }

        public Image GetCaptchaImage()
        {
            Image result = null;

            if (!this.mutex.WaitOne(0, false))
            {
                throw new BeanfunIsBusyException();
            }

            try
            {
                string captchaUrl = "https://tw.newlogin.beanfun.com/login/BotDetectCaptcha.ashx?get=image&c=c_login_idpass_form_samplecaptcha&t=" + this.captchaId + "&d=" + UnixTimeStamp.Now().ToString();
                byte[] buffer = this.webClient.DownloadData(captchaUrl);
                result = Image.FromStream(new MemoryStream(buffer));
            }
            catch (Exception) { }

            this.mutex.ReleaseMutex();
            return result;
        }

        public bool Login(string account, string password, string captchaCode)
        {
            try
            {
                NameValueCollection data = new NameValueCollection();
                data.Add("__EVENTTARGET", "");
                data.Add("__EVENTARGUMENT", "");
                data.Add("__VIEWSTATE", login_VIEWSTATE);
                data.Add("__EVENTVALIDATION", login_EVENTVALIDATION);
                data.Add("t_AccountID", User);
                data.Add("t_Password", Pass);
                data.Add("CodeTextBox", Captcha);
                data.Add("btn_login.x", "46");
                data.Add("btn_login.y", "31");
                data.Add("LBD_VCID_c_login_idpass_form_samplecaptcha", login_captchaID);

                string ret = Encoding.UTF8.GetString(spwc.UploadValues(login_url, data));

                if (spwc.ResponseUri.ToString().Contains("id-pass_form.aspx"))
                {
                    string error_message = "";
                    try
                    {
                        Regex regErrorMessage = new Regex(@"MsgBox.Show\('([^']+)'\)");
                        error_message = regErrorMessage.Match(ret).Groups[1].Value;
                    }
                    catch (Exception) { throw new BeanfunLoginFailedException(); }

                    throw new BeanfunLoginFailedException(error_message);
                }

                Regex regAuthkey = new Regex(@"AuthKey\.value = \x22(\w+)\x22");
                Regex regSessionkey = new Regex(@"SessionKey\.value = \x22(\w+)\x22");

                data.Clear();
                try
                {
                    data.Add("SessionKey", regSessionkey.Match(ret).Groups[1].Value);
                    data.Add("AuthKey", regAuthkey.Match(ret).Groups[1].Value);
                }
                catch (Exception) { throw new BeanfunLoginFailedException(); }

                ret = Encoding.UTF8.GetString(spwc.UploadValues("https://tw.new.beanfun.com/beanfun_block/bflogin/return.aspx", data));

                Regex regSuccess = new Regex(@"https?://tw\.new\.beanfun\.com/default\.aspx");

                if (!regSuccess.IsMatch(spwc.ResponseUri.ToString()))
                    throw new BeanfunLoginFailedException();

                /*
                 * In Cookie
                 *     bfWebToken
                 *     GET /beanfun_block/auth.aspx?channel=game_zone&page_and_query=game_start.aspx%3Fservice_code_and_region%3D610074_T9&web_token=3eb8306996a34ed7b8556bde31d0b0a3 HTTP/1.1
                 */

                CookieCollection cookies = cc.GetCookies(new Uri("http://tw.new.beanfun.com"));
                foreach (Cookie cookie in cookies)
                    if (cookie.Name == "bfWebToken")
                        this.webtoken = cookie.Value;
            }
            catch (NotSupportedException)
            {
                this.StillAlive = false;
                throw new BeanfunIsBusyException("正在處理其他的事情");
            }
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
