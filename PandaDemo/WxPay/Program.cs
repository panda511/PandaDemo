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
            string ip = null;// "127.0.0.1";
            string openId = "openId";
            string attach = "充值订单";

            WxAppPay pay = new WxAppPay();
            var payParam = pay.GetPayParameter(orderNo, amount, body, ip, null, attach);

            Console.WriteLine(payParam.ToJson());

            //IWxPay pay2 = new WxJsApiPay();
            //var payParam2 = pay2.GetPayParameter(orderNo, amount, body, ip, openId);

            return;


            string appId = "wx2428e34e0e7dc6ef";
            string key = "e10adc3849ba56abbe56e056f20f883e";

            string stamp = "1525663034";
            string nonceStr = "3669A032E96C56D111292833CB51F79F";
            string package = "prepay_id=wx1234567890";
            string signType = "MD5";

            string sign = TenPayV3.GetJsPaySign(appId, stamp, nonceStr, package, key, signType);
            //D8B716043EC84E0503EA9D303B017336

            //string sign2 = new WxJsApiPay().GetPaySign(appId, package, nonceStr, stamp, signType, key);

            //Console.WriteLine(sign);
            //Console.WriteLine(sign2);


            string s = Guid.NewGuid().ToString();//.Replace("-", ""); 
            string s2 = Guid.NewGuid().ToString("N");
            string s3 = String2.GetGuid();



            Console.WriteLine(s);
            Console.WriteLine(s2);
            Console.WriteLine(s3);

            Console.Read();
        }

        static void Main22(string[] args) 
        {
            string returnCode = "FAIL";
            string returnMsg = "FAIL";

            WxPayNotify notify = new WxPayNotify();
            if (notify.IsSafe)
            {
                //交易成功
                if (notify.ResultCode.ToLower() == "success")
                {
                    if (notify.Attach == PayOrderType.RechargeOrder.ToString())
                    {
                        decimal tradeAmount = notify.TotalFee / 100m;
                        bool success = HandleRechargeOrderNotify(notify.OutTradeNo, notify.TransactionId, tradeAmount);
                        if (success)
                        {
                            returnCode = "SUCCESS";
                            returnMsg = "OK";
                        }
                    }
                }
            }

            #region result
            string result = @"
                <xml>
                    <return_code><![CDATA[{0}]]></return_code>
                    <return_msg><![CDATA[{1}]]></return_msg>
                </xml>
            ";
            result = string.Format(result, returnCode, returnMsg);
            #endregion

            //return Content(result, "text/xml");
        }

        private static bool HandleRechargeOrderNotify(string outOrderNo, string tradeNo, decimal tradeAmout)
        {
            bool success = false;

            //var order = service.GetOrder(outOrderNo);
            //if (order != null)
            //{
            //    //充值订单还没处理过
            //    if (order.Type == 2 && order.Status == 0)
            //    {
            //        //判断订单金额和回调通知金额是否一致
            //        if (order.Money == tradeAmout)
            //        {
            //            success = 进行更新订单支付状态等业务操作;
            //        }
            //    }
            //}

            return success;
        }

    }

    public enum PayOrderType
    {
        ProductOrder,

        VipOrder,

        RechargeOrder
    }

   

}
