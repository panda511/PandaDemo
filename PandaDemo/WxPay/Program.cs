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
            int price = 360;
            string body = "abcdefg";
            var payParam = new WxAppPayParameterOutDto();

            WxAppPay pay = new WxAppPay();
            var wxOrder = pay.CreatePrePayOrder(orderNo, price, body);
            if (wxOrder != null)
            {
                string timeStamp = TenPayV3Util.GetTimestamp();
                payParam.AppId = wxOrder.appid;
                payParam.PartnerId = wxOrder.mch_id;
                payParam.PrepayId = wxOrder.prepay_id;
                payParam.Package = "Sign=WXPay";
                payParam.NonceStr = wxOrder.nonce_str;
                payParam.TimeStamp = timeStamp;
                payParam.Sign = pay.GetPaySign(wxOrder.prepay_id, wxOrder.nonce_str, timeStamp);
            }

            Console.WriteLine(payParam.ToJson());
            Console.Read();
        }
    }

    /// <summary>
    /// 微信APP支付
    /// </summary>
    public class WxAppPay
    {
        string appId = "";
        string mchId = "";
        string key = "";
        string NotifyUrl = "";

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        public UnifiedorderResult CreatePrePayOrder(string orderNo, int price, string body)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string ip = ""; //HttpContext.Current.Request.UserHostAddress;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(appId, mchId, body, orderNo, price, ip, NotifyUrl, TenPayV3Type.APP, null, key, nonceStr);
            var result = TenPayV3.Unifiedorder(xmlDataInfo);

            //下单失败
            if (!result.IsResultCodeSuccess() || !result.IsReturnCodeSuccess())
            {
                //todo:记日志

                Console.WriteLine(result.ToJson());
                Console.WriteLine("");
                Console.WriteLine("");

                return null;
            }

            return result;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        public string GetPaySign(string prepayId, string nonceStr, string timeStamp)
        {
            //设置支付参数
            string package = "Sign=WXPay";
            string signType = "MD5";
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

    /// <summary>
    /// 微信JSAPI支付(公众号)
    /// </summary>
    public class WxJsApiPay
    {
        string appId = "";
        string mchId = "";
        string key = "";
        string NotifyUrl = "";

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        public UnifiedorderResult CreatePrePayOrder(string orderNo, int price, string body, string openId)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string ip = HttpContext.Current.Request.UserHostAddress;

            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(appId, mchId, body, orderNo, price, ip, NotifyUrl, TenPayV3Type.JSAPI, openId, key, nonceStr);
            var result = TenPayV3.Unifiedorder(xmlDataInfo);

            //下单失败
            if (!result.IsResultCodeSuccess() || !result.IsReturnCodeSuccess())
            {
                //todo:记日志

                return null;
            }

            return result;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        public string GetPaySign(string prepayId, string nonceStr, string timeStamp)
        {
            string package = string.Format("prepay_id={0}", prepayId);
            var paySign = TenPayV3.GetJsPaySign(appId, timeStamp, nonceStr, package, key);
            return paySign;
        }
    }


    /// <summary>
    /// 微信APP支付参数
    /// </summary>
    public class WxAppPayParameterOutDto
    {
        /// <summary>
        /// 微信开放平台审核通过的应用APPID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string PartnerId { get; set; }

        /// <summary>
        /// 预支付交易会话ID
        /// </summary>
        public string PrepayId { get; set; }

        /// <summary>
        /// 扩展字段 固定值Sign=WXPay
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }

    /// <summary>
    /// 微信JSAPI支付(公众号支付)参数
    /// </summary>
    public class WxJsApiPayParameterOutDto
    {
        /// <summary>
        /// 公众号id(商户注册具有支付权限的公众号成功后即可获得)
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 扩展字段 统一下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 签名方式 默认为MD5
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string PaySign { get; set; }
    }
}
