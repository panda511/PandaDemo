using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliPay
{
    class Program
    {
        static void Main2(string[] args)
        {
            AliPay pay = new AliPay();
            decimal amount = 0.03m;
            string orderNo = "123456789";
            string subject = "iphone7 黑色 64G";
            string body = "京东商城"+ subject+ orderNo;
            string str = pay.GetWebPayParameter(amount, orderNo, subject, body);
            Console.WriteLine(str);
            Console.Read();
        }

        static void Main(string[] args)
        {
            string result = "fail";

            AliPayNotify notify = new AliPayNotify();
            if (notify.IsSafe)
            {
                //交易支付成功
                if (notify.TradeStatus == AliPayTradeStatus.TRADE_SUCCESS.ToString())
                {
                    if (notify.PassbackParams == PayOrderType.RechargeOrder.ToString())
                    {
                        bool success = HandleRechargeOrderNotify(notify.OutTradeNo, notify.TradeNo, notify.TotalAmount);
                        if (success)
                        {
                            result = "success";
                        }
                    }
                }
            }

            //return Content(result);
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
