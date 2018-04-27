using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay
{
    /// <summary>
    /// 微信APP支付
    /// </summary>
    public class WxAppPay: IWxPay
    {
        public static readonly string AppId = "";
        public static readonly string MchId = "";
        public static readonly string Key = "";
        public static readonly string NotifyUrl = "";

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <returns></returns>
        public WxPayParameter GetPayParameter(string orderNo, int amout, string body, string ip, string openId = null)
        {
            WxPayParameter param = null;

            var wxOrder = CreatePrePayOrder(orderNo, amout, body, ip);
            if (wxOrder.IsReturnCodeSuccess() && wxOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = wxOrder.appid,
                    PartnerId = wxOrder.mch_id,
                    PrepayId = wxOrder.prepay_id,
                    NonceStr = wxOrder.nonce_str,
                    TimeStamp = TenPayV3Util.GetTimestamp(),
                    Package = "Sign=WXPay",
                };

                param.Sign = GetPaySign(param.PrepayId, param.Package, param.NonceStr, param.TimeStamp, param.SignType);
            }

            return param;
        }

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(string orderNo, int amount, string body, string ip, string openId = null)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            var payType = TenPayV3Type.APP;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(AppId, MchId, body, orderNo, amount, ip, NotifyUrl, payType, openId, Key, nonceStr);
            var order = TenPayV3.Unifiedorder(xmlDataInfo);
            return order;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        private string GetPaySign(string prepayId, string package, string nonceStr, string timeStamp, string signType)
        {
            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("appId", AppId);
            paySignReqHandler.SetParameter("partnerId", MchId);
            paySignReqHandler.SetParameter("prepayId", prepayId);
            paySignReqHandler.SetParameter("package", package);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("signType", signType);
            var paySign = paySignReqHandler.CreateMd5Sign("key", Key);
            return paySign;
        }

    }
}
