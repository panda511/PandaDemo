using EasyHttp.Http;
using Extension;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;

namespace WxPay
{
    /// <summary>
    /// 微信JSAPI支付类
    /// </summary>
    public class WxJsApiPay : IWxPay
    {
        public static readonly string AppId = "";
        public static readonly string MchId = "";
        public static readonly string Key = ""; //支付密钥
        public static readonly string NotifyUrl = "";
        public static readonly string AppSecret = ""; //唯一凭证密钥，仅JSAPI支付的时候需要配置，用于获取openId

        public const string Domain = "https://api.mch.weixin.qq.com/";

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <returns></returns>
        public WxPayParameter GetPayParameter(string orderNo, int amount, string body, string ip, string openId, string attach)
        {
            WxPayParameter param = null;

            var wxOrder = CreatePrePayOrder(orderNo, amount, body, openId, ip, attach);
            if (wxOrder.IsReturnCodeSuccess() && wxOrder.IsResultCodeSuccess())
            {
                param = new WxPayParameter()
                {
                    AppId = wxOrder.appid,
                    PartnerId = wxOrder.mch_id,
                    PrepayId = wxOrder.prepay_id,
                    NonceStr = wxOrder.nonce_str,
                    Timestamp = DateTime2.GetTimestamp().ToString(),
                    Package = string.Format("prepay_id={0}", wxOrder.prepay_id)
                };

                //param.Sign = TenPayV3.GetJsPaySign(AppId, param.TimeStamp, param.NonceStr, param.Package, Key, param.SignType);
                param.Sign = GetPaySign(param.NonceStr, param.Package, param.SignType, param.Timestamp);
            }
            else
            {
                //预支付订单创建失败，记日志
            }

            return param;
        }

        /// <summary>
        /// 创建微信预支付交易单
        /// </summary>
        private UnifiedorderResult CreatePrePayOrder(string orderNo, int amount, string body, string openId, string ip, string attach)
        {
            string nonceStr = String2.GetGuid();
            var payType = TenPayV3Type.JSAPI;

            var param = new TenPayV3UnifiedorderRequestData(AppId, MchId, body, orderNo, amount, ip, NotifyUrl, payType, openId, Key, nonceStr);
            param.Attach = attach;
            var order = TenPayV3.Unifiedorder(param);
            return order;
        }

        private void CreatePrePayOrder2(string orderNo, int amount, string body, string openId, string ip, string attach)
        {
            string nonceStr = String2.GetGuid();

            string p = "appid={0}&attach={1}&body={2}&mch_id={3}&nonce_str={4}&notify_url={5}&openid={6}&out_trade_no={7}&spbill_create_ip={8}&total_fee={9}&trade_type=JSAPI";
            string str = string.Format(p, AppId, attach, body, MchId, nonceStr, NotifyUrl, openId, orderNo, ip, amount);
            string sign = str.ToMd5().ToUpper();

            var param = new
            {
                appid = AppId,
                attach = attach, //附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
                body = body,
                mch_id = MchId,
                nonce_str = nonceStr,
                notify_url = NotifyUrl,
                openid = openId,
                out_trade_no = orderNo,
                spbill_create_ip = ip,
                total_fee = amount,
                trade_type = "JSAPI",
                sign = sign

                //device_info = "web", //终端设备号
                //detail = "", //商品详细描述
                //fee_type = "CNY", //币种
                //time_start = "yyyyMMddHHmmss",
                //time_expire = "yyyyMMddHHmmss",
                //goods_tag = "WXG", //订单优惠标记，使用代金券或立减优惠功能时需要的参数
                //product_id = "", //trade_type=NATIVE时（即扫码支付），此参数必传。此参数为二维码中包含的商品ID，商户自行定义。
                //sign_type="MD5",
                //limit_pay= "no_credit", //上传此参数no_credit--可限制用户不能使用信用卡支付
                //scene_info = "", //该字段用于上报场景信息，目前支持上报实际门店信息。该字段为JSON对象数据
            };

            var http = new HttpClient();
            string url = Domain + "pay/unifiedorder";
            var resp = http.Post(url, param, HttpContentTypes.ApplicationXml);
            string str = resp.RawText;
        }


        /// <summary>
        /// 获取支付签名
        /// </summary>
        private string GetPaySign(string nonceStr, string package, string signType, string timeStamp) 
        {
            string param = "appId={0}&nonceStr={1}&package={2}&signType={3}&timeStamp={4}&key={5}";
            string str = string.Format(param, AppId, nonceStr, package, signType, timeStamp, Key);
            string sign = str.ToMd5().ToUpper();
            return sign;
        }
    }
}
