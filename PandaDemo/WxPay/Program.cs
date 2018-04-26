using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Extension;
using Extension.Extention;

namespace WxPay
{
    class Program
    {
        static void Main(string[] args)
        {
            string orderNo = "123456789";
            int amount = 360;
            string body = "abcdefg";
            string ip = "127.0.0.1";
            string openId = "openId";

            IWxPay pay = new WxAppPay();
            var payParam = pay.GetPayParameter(orderNo, amount, body, ip, null);

            IWxPay pay2 = new WxJsApiPay();
            var payParam2 = pay.GetPayParameter(orderNo, amount, body, ip, openId);

            Console.WriteLine(payParam2.ToJson());
            Console.Read();
        }
    }
}
