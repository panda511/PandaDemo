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
            string attach = "充值订单";

            IWxPay pay = new WxAppPay();
            var payParam = pay.GetPayParameter(orderNo, amount, body, ip, null, attach);

            IWxPay pay2 = new WxJsApiPay();
            var payParam2 = pay.GetPayParameter(orderNo, amount, body, ip, openId);

            Console.WriteLine(payParam2.ToJson());
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
