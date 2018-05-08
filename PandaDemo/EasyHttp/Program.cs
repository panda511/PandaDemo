using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using EasyHttp.Http;

namespace EasyHttp
{
    class Program
    {
        /*CheckValidationResult的定义*/
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        private static CookieContainer cookie = new CookieContainer();


        static void Main(string[] args)
        {
            var http = new HttpClient();

            //设置请求相关参数
            http.Request.Accept = HttpContentTypes.ApplicationJson;
            http.Request.AcceptCharSet = "";
            http.Request.AcceptEncoding = "UTF-8";
            http.Request.AcceptLanguage = "";
            http.Request.AddExtraHeader("header", "value");
            http.Request.AllowAutoRedirect = false;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);//验证服务器证书回调自动验证 

            X509Certificate2 certificate = new X509Certificate2("证书地址", "证书密码", X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            http.Request.ClientCertificates.Add(certificate);

            http.Request.ContentEncoding = "";
            //http.Request.ContentLength = "";
            http.Request.ContentType = "text/xml";
            http.Request.Cookies.Add(new Cookie("name", "value"));
            http.Request.Data = null;
            http.Request.Date = DateTime.Now;
            http.Request.DisableAutomaticCompression = false;
            http.Request.Expect = false;
            http.Request.ForceBasicAuth = false;
            http.Request.From = "";
            http.Request.Host = "";
            http.Request.IfMatch = "";
            http.Request.IfModifiedSince = DateTime.Now;
            http.Request.IfRange = "";
            http.Request.KeepAlive = false;
            http.Request.MaxForwards = 9;
            http.Request.Method = HttpMethod.POST;
            http.Request.MultiPartFileData = null;
            http.Request.MultiPartFormData = null;
            http.Request.ParametersAsSegments = false;
            http.Request.PersistCookies = false;
            http.Request.PutFilename = "";
            http.Request.Range = 90;
            //http.Request.RawHeaders = null;
            http.Request.Referer = "";
            http.Request.Timeout = 5000;
            http.Request.Uri = "";
            http.Request.UserAgent = "";

            http.Request.SetCacheControlToNoCache();
            http.Request.SetBasicAuthentication("jim", "123456");

            var param = new
            {
                user_id = "39183",
                goods_id_list = "491,499,489,497",
                goods_num_list = "1,1,1,2",
                CouponID = "0"
            };
         
            string url = "http://api.leqin-gf.com/api/Distribution/GetCategory";
            var response = http.Post(url, param, HttpContentTypes.ApplicationJson);
            response = http.Post(url, param, HttpContentTypes.ApplicationXml);

            //var result = response.DynamicBody;
            //Console.WriteLine(result.msg);

            var result = response.StaticBody<RespResult>();
            Console.WriteLine(result.code);
            Console.WriteLine(result.msg);
            Console.WriteLine(result.time);
            Console.WriteLine(response.RawText);
        }
    }

    class RespResult
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string time { get; set; }
        public Data data { get; set; }
    }

    class Data
    {
        public string DeliverFee { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string OrderTotalPrice { get; set; }
        public string OrdertTotalDiscount { get; set; }
    }


}
