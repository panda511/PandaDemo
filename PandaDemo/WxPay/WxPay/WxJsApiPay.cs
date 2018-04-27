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
        public static readonly string AppId = "";
        public static readonly string MchId = "";
        public static readonly string Key = "";
        public static readonly string NotifyUrl = "";
        public static readonly string AppSecret = ""; //唯一凭证密钥，仅JSAPI支付的时候需要配置，用于获取openId

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <returns></returns>
        public WxPayParameter GetPayParameter(string orderNo, int amount, string body, string ip, string openId)
        {
            WxPayParameter param = null;

            var wxOrder = CreatePrePayOrder(orderNo, amount, body, openId, ip);
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

                param.Sign = TenPayV3.GetJsPaySign(AppId, param.TimeStamp, param.NonceStr, param.Package, Key, param.SignType);
            }

            return param;
        }

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(string orderNo, int amount, string body, string openId, string ip)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            var payType = TenPayV3Type.JSAPI;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(AppId, MchId, body, orderNo, amount, ip, NotifyUrl, payType, openId, Key, nonceStr);
            var order = TenPayV3.Unifiedorder(xmlDataInfo);
            return order;
        }
    }
}
