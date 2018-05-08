using Extension;
using Extension.Extention;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;

namespace WxPay
{
    /// <summary>
    /// 微信APP支付类
    /// </summary>
    public class WxAppPay : IWxPay
    {
        public static readonly string AppId = "";
        public static readonly string MchId = "";
        public static readonly string Key = ""; //支付密钥
        public static readonly string NotifyUrl = "";

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <returns></returns>
        public WxPayParameter GetPayParameter(string orderNo, int amount, string body, string ip, string openId, string attach)
        {
            WxPayParameter param = null;

            var wxOrder = CreatePrePayOrder(orderNo, amount, body, ip, openId, attach);
            if (wxOrder.IsReturnCodeSuccess() && wxOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = wxOrder.appid,
                    PartnerId = wxOrder.mch_id,
                    PrepayId = wxOrder.prepay_id,
                    NonceStr = wxOrder.nonce_str,
                    Timestamp = DateTime2.GetTimestamp().ToString(),
                    Package = "Sign=WXPay",
                };

                param.Sign = GetPaySign(param.NonceStr, param.Package, param.PrepayId, param.Timestamp);
            }
            else
            {
                //预支付订单创建失败，记日志
                Console.WriteLine(wxOrder.ToJson());
            }

            return param;
        }

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(string orderNo, int amount, string body, string ip, string openId, string attach)
        {
            string nonceStr = String2.GetGuid();
            var payType = TenPayV3Type.APP;

            var param = new TenPayV3UnifiedorderRequestData(AppId, MchId, body, orderNo, amount, ip, NotifyUrl, payType, openId, Key, nonceStr);
            param.Attach = attach;
            var order = TenPayV3.Unifiedorder(param);
            return order;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        private string GetPaySign2(string nonceStr, string package, string prepayId, string timestamp)
        {
            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("appid", AppId);
            paySignReqHandler.SetParameter("partnerid", MchId);
            paySignReqHandler.SetParameter("prepayid", prepayId);
            paySignReqHandler.SetParameter("package", package);
            paySignReqHandler.SetParameter("noncestr", nonceStr);
            paySignReqHandler.SetParameter("timestamp", timestamp);
            //paySignReqHandler.SetParameter("signType", signType);
            var paySign = paySignReqHandler.CreateMd5Sign("key", Key);
            return paySign;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        public string GetPaySign(string nonceStr, string package, string prepayId, string timestamp)
        {
            string param = "appid={0}&noncestr={1}&package={2}&partnerid={3}&prepayid={4}&timeStamp={5}&key={6}";
            string str = string.Format(param, AppId, nonceStr, package, MchId, prepayId, timestamp, Key);
            string sign = str.ToMd5().ToUpper();
            return sign;
        }

    }
}
