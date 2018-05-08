using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Http
{
    class Program
    {
        private static CookieContainer cookie = new CookieContainer();

        static void Main(string[] args)
        {
        }

        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="serverUrl">服务Url</param>
        /// <param name="postData">传递参数，格式如“key1=value&key2=value2”</param>
        /// <returns></returns>
        public static string HttpGetConnectToServer(string serverUrl, string postData = "")
        {
            //创建请求  
            var request = (HttpWebRequest)WebRequest.Create(serverUrl + "?" + postData);
            request.Method = "GET";
            //设置上传服务的数据格式  
            request.ContentType = "application/x-www-form-urlencoded";
            //请求的身份验证信息为默认  
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间  
            request.Timeout = 60000;
            //设置cookie
            request.CookieContainer = cookie;

            try
            {
                //读取返回消息
                return GetResponseAsString(request);
            }
            catch (Exception ex)
            {
                //var result = new ServerResult();
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// Post获取数据
        /// </summary>
        /// <param name="serverUrl">服务Url</param>
        /// <param name="postData">传递参数，格式如“key1=value&key2=value2”</param>
        /// <returns></returns>
        public static string HttpPostConnectToServer(string serverUrl, string postData)
        {
            var dataArray = Encoding.UTF8.GetBytes(postData);
            //创建请求  
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //设置上传服务的数据格式  
            request.ContentType = "application/x-www-form-urlencoded";
            //请求的身份验证信息为默认  
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间  
            request.Timeout = 600000;
            //设置cookie
            request.CookieContainer = cookie;
            //创建输入流  
            Stream dataStream;

            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败  
            }
            //发送请求  
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息  
            //string res;
            try
            {
                //读取返回消息
                return GetResponseAsString(request);
            }
            catch (Exception ex)
            {
                //连接服务器失败
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// Post获取数据
        /// </summary>
        /// <param name="serverUrl">服务Url</param>
        /// <param name="postData">传递参数，格式如“key1=value&key2=value2”</param>
        /// <returns></returns>
        public static string HttpPostConnectToServer2(string serverUrl, string postData)
        {
            var dataArray = Encoding.UTF8.GetBytes(postData);
            //创建请求  
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
            request.Method = "POST";
            request.ContentLength = dataArray.Length;
            //设置上传服务的数据格式  
            request.ContentType = "application/x-www-form-urlencoded";
            //请求的身份验证信息为默认  
            request.Credentials = CredentialCache.DefaultCredentials;
            //请求超时时间  
            request.Timeout = 600000;
            //设置cookie
            request.CookieContainer = cookie;
            //创建输入流  
            Stream dataStream;

            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败  
            }
            //发送请求  
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息  
            //string res;
            try
            {
                //读取返回消息
                return GetResponseAsString(request);
            }
            catch (Exception ex)
            {
                //连接服务器失败
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// Post获取数据
        /// </summary>
        /// <param name="serverUrl">服务Url</param>
        /// <param name="postData">传递参数，格式如“{"key1":"value","key2":"value2"}”</param>
        /// <returns></returns>
        public static string HttpPostJsonConnectToServer(string serverUrl, string postData)
        {
            var dataArray = Encoding.UTF8.GetBytes(postData);

            //创建请求  
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUrl);

            request.Method = "POST";
            request.ContentLength = dataArray.Length;

            //设置上传服务的数据格式 json格式的传递参数
            request.ContentType = "application/json";

            //请求的身份验证信息为默认  
            request.Credentials = CredentialCache.DefaultCredentials;

            //请求超时时间  
            request.Timeout = 600000;

            //设置cookie
            request.CookieContainer = cookie;

            //创建输入流  
            Stream dataStream;
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败  
            }

            //发送请求  
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();

            try
            {
                //读取返回消息
                return GetResponseAsString(request);
            }
            catch (Exception ex)
            {
                return "{\"error\":\"connectToServer\",\"error_description\":\"" + ex.Message + "\"}";
            }
        }


        private static string GetResponseAsString(HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string res = reader.ReadToEnd();
                reader.Close();//关闭读取流
                response.Close();//关闭响应流
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
