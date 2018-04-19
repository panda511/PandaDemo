using EasyHttp.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var param = new
            {
                user_id = "39183",
                goods_id_list = "491,499,489,497",
                goods_num_list = "1,1,1,2",
                CouponID = "0"
            };

            string url = "http://api.leqin-gf.com/api/Distribution/GetCategory";
            var http = new HttpClient();
            http.Request.AcceptEncoding = "UTF-8";
            var response = http.Post(url, param, HttpContentTypes.ApplicationJson);

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
