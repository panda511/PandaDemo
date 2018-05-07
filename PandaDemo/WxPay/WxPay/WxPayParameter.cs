using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay
{
    /// <summary>
    /// 微信支付参数
    /// </summary>
    public class WxPayParameter
    {
        /// <summary>
        /// 微信开放平台审核通过的应用APPID / 公众号id(商户注册具有支付权限的公众号成功后即可获得)
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
        /// 扩展字段 APP为固定值Sign=WXPay / JsApi为统一下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 签名方式 默认为MD5
        /// </summary>
        public string SignType { get; } = "MD5";
    }
}
