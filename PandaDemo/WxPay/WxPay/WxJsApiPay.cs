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
    public class WxJsApiPay : IWxPay
    {
        string appId = "";// (ConfigurationManager.AppSettings["WxJsApiPayAppId"]).Trim2();
        string mchId = "";//(ConfigurationManager.AppSettings["WxJsApiPayMchId"]).Trim2();
        string key = "";//(ConfigurationManager.AppSettings["WxJsApiPayKey"]).Trim2(); //支付密钥
        string notifyUrl = "";//(ConfigurationManager.AppSettings["WxPayNotifyUrl"]).Trim2();
        string appSecret = "";//(ConfigurationManager.AppSettings["WxJsApiPayAppSecret"]).Trim2(); //唯一凭证密钥，仅JSAPI支付的时候需要配置，用于获取openId

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <returns></returns>
        public WxPayParameter GetPayParameter(string orderNo, int amout, string body, string ip, string openId)
        {
            WxPayParameter param = null;

            var wxOrder = CreatePrePayOrder(orderNo, amout, body, openId, ip);
            if (wxOrder.IsReturnCodeSuccess() && wxOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = wxOrder.appid,
                    PartnerId = wxOrder.mch_id,
                    PrepayId = wxOrder.prepay_id,
                    NonceStr = wxOrder.nonce_str,
                    TimeStamp = TenPayV3Util.GetTimestamp(),
                    Package = string.Format("prepay_id={0}", wxOrder.prepay_id),
                };

                param.Sign = TenPayV3.GetJsPaySign(appId, param.TimeStamp, param.NonceStr, param.Package, key, param.SignType);
            }

            return param;
        }

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(string orderNo, int amout, string body, string openId, string ip)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            var payType = TenPayV3Type.JSAPI;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(appId, mchId, body, orderNo, amout, ip, notifyUrl, payType, openId, key, nonceStr);
            var order = TenPayV3.Unifiedorder(xmlDataInfo);
            return order;
        }
    }
}
