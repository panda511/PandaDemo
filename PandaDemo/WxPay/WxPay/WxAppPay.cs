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
        string appId = "";//(ConfigurationManager.AppSettings["WxAppPayAppId"]).Trim2();
        string mchId = "";//(ConfigurationManager.AppSettings["WxAppPayMchId"]).Trim2();
        string key = "";//(ConfigurationManager.AppSettings["WxAppPayKey"]).Trim2(); //支付密钥
        string notifyUrl = "";//(ConfigurationManager.AppSettings["WxPayNotifyUrl"]).Trim2();
        string appSecret = "";//(ConfigurationManager.AppSettings["WxAppPayAppSecret"]).Trim2(); //唯一凭证密钥

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

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(appId, mchId, body, orderNo, amount, ip, notifyUrl, payType, openId, key, nonceStr);
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
            paySignReqHandler.SetParameter("appId", appId);
            paySignReqHandler.SetParameter("partnerId", mchId);
            paySignReqHandler.SetParameter("prepayId", prepayId);
            paySignReqHandler.SetParameter("package", package);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("signType", signType);
            var paySign = paySignReqHandler.CreateMd5Sign("key", key);
            return paySign;
        }

    }
}
