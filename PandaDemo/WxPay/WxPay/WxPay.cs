using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using ZCommon;
using ZCommon.Extention;

namespace WxPay
{
    /// <summary>
    /// 微信支付类
    /// </summary>
    public class WxPay
    {
        public static readonly string AppAppId = ConfigurationManager.AppSettings["WxPay_App_AppId"];
        public static readonly string AppMchId = ConfigurationManager.AppSettings["WxPay_App_MchId"];
        public static readonly string AppKey = ConfigurationManager.AppSettings["WxPay_App_Key"]; //支付密钥

        public static readonly string JsApiAppId = ConfigurationManager.AppSettings["WxPay_JsApi_AppId"];
        public static readonly string JsApiMchId = ConfigurationManager.AppSettings["WxPay_JsApi_MchId"];
        public static readonly string JsApiKey = ConfigurationManager.AppSettings["WxPay_JsApi_Key"];
        public static readonly string JsApiAppSecret = ConfigurationManager.AppSettings["WxPay_JsApi_AppSecret"]; //唯一凭证密钥，仅JSAPI支付的时候需要配置，用于获取openId

        public static readonly string NotifyUrl = ConfigurationManager.AppSettings["WxPay_NotifyUrl"];

        #region App

        /// <summary>
        /// 获取APP支付参数
        /// </summary>
        public WxPayParameter GetAppPayParameter(string orderNo, int amount, string body, string ip, string attach = null)
        {
            WxPayParameter param = null;

            var preOrder = CreatePrePayOrder(TenPayV3Type.APP, orderNo, amount, body, ip, null, null, attach);

            if (preOrder.IsReturnCodeSuccess() && preOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = preOrder.appid,
                    PartnerId = preOrder.mch_id,
                    PrepayId = preOrder.prepay_id,
                    NonceStr = preOrder.nonce_str,
                    Timestamp = DateTime2.GetTimestamp().ToString(),
                    Package = "Sign=WXPay",
                };

                param.Sign = GetAppPaySign(param.NonceStr, param.Package, param.PrepayId, param.Timestamp);
            }
            else
            {
                //预支付订单创建失败，记日志
            }

            return param;
        }

        /// <summary>
        /// 获取APP支付签名
        /// </summary>
        private string GetAppPaySign(string nonceStr, string package, string prepayId, string timestamp)
        {
            string param = "appid={0}&noncestr={1}&package={2}&partnerid={3}&prepayid={4}&timestamp={5}&key={6}";
            string str = string.Format(param, AppAppId, nonceStr, package, AppMchId, prepayId, timestamp, AppKey);
            string sign = str.ToMd5().ToUpper();
            return sign;
        }

        #endregion

        #region JsApi

        /// <summary>
        /// 获取JsApi支付参数
        /// </summary>
        public WxPayParameter GetJsApiPayParameter(string orderNo, int amount, string body, string ip, string openId, string attach = null)
        {
            WxPayParameter param = null;

            var preOrder = CreatePrePayOrder(TenPayV3Type.JSAPI, orderNo, amount, body, ip, openId, null, attach);

            if (preOrder.IsReturnCodeSuccess() && preOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = preOrder.appid,
                    PartnerId = preOrder.mch_id,
                    PrepayId = preOrder.prepay_id,
                    NonceStr = preOrder.nonce_str,
                    Timestamp = DateTime2.GetTimestamp().ToString(),
                    Package = string.Format("prepay_id={0}", preOrder.prepay_id),
                };

                param.Sign = GetJsApiPaySign(param.NonceStr, param.Package, param.SignType, param.Timestamp);
            }
            else
            {
                //预支付订单创建失败，记日志
            }

            return param;
        }

        /// <summary>
        /// 获取JsApi支付签名
        /// </summary>
        private string GetJsApiPaySign(string nonceStr, string package, string signType, string timeStamp)
        {
            string template = "appId={0}&nonceStr={1}&package={2}&signType={3}&timeStamp={4}&key={5}";
            string str = string.Format(template, JsApiAppId, nonceStr, package, signType, timeStamp, JsApiKey);
            string sign = str.ToMd5().ToUpper();
            return sign;
        }

        #endregion

        #region Native

        /// <summary>
        /// 获取扫码支付的二维码图片链接地址(将链接用二维码工具生成二维码打印出来，顾客可以用微信扫码支付, 有效期为2小时)
        /// </summary>
        public string GetQrCodeUrl(string orderNo, int amount, string body, string ip, string productId, string attach = null)
        {
            string url = string.Empty;

            var preOrder = CreatePrePayOrder(TenPayV3Type.NATIVE, orderNo, amount, body, ip, null, productId, attach);

            if (preOrder.IsReturnCodeSuccess() && preOrder.IsResultCodeSuccess())
            {
                url = preOrder.code_url;
            }

            return url;
        }

        #endregion


        /// <summary>
        /// 创建预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(TenPayV3Type payType, string orderNo, int amount, string body, string ip, string openId, string productId, string attach)
        {
            string nonceStr = String2.GetGuid();

            string appId = JsApiAppId;
            string mchId = JsApiMchId;
            string key = JsApiKey;

            if (payType == TenPayV3Type.APP)
            {
                appId = AppAppId;
                mchId = AppMchId;
                key = AppKey;
            }

            var param = new TenPayV3UnifiedorderRequestData(appId, mchId, body, orderNo, amount, ip, NotifyUrl, payType, openId, key, nonceStr, productId: productId, attach: attach);
            var order = TenPayV3.Unifiedorder(param);
            return order;
        }

    }
}
