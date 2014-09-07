using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NewBeanfunStarter
{
    public class CustomWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }
        public Uri ResponseUri { get; private set; }

        public CustomWebClient() : base()
        {
            this.CookieContainer = new CookieContainer();
            this.ResponseUri = null;
        }

        public CustomWebClient(CookieContainer CookieContainer) : base()
        {
            this.CookieContainer = CookieContainer;
            this.ResponseUri = null;
        }

        public string DownloadString(string Uri, Encoding Encoding)
        {
            return Encoding.GetString(this.DownloadData(Uri));
        }

        public string DownloadString(Uri Uri, Encoding Encoding)
        {
            return Encoding.GetString(this.DownloadData(Uri));
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null) webRequest.CookieContainer = this.CookieContainer;
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            this.ResponseUri = response.ResponseUri;
            return response;
        }
    }
}
