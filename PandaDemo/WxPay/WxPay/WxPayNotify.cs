using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZCommon.Extention;

namespace WxPay
{
    /// <summary>
    /// 微信支付回调通知
    /// </summary>
    public class WxPayNotify
    {
        string appId = string.Empty;
        ResponseHandler resp = null;

        /// <summary>
        /// 回调通知是否安全
        /// </summary>
        public bool IsSafe
        {
            get
            {
                bool safe = false;

                if (ReturnCode.ToLower() == "success")
                {
                    if (AppId == appId) //验证商户
                    {
                        safe = resp.IsTenpaySign(); //验证参数
                    }
                }

                return safe;
            }
        }

        public WxPayNotify()
        {
            resp = new ResponseHandler(HttpContext.Current);

            if (TradeType == TenPayV3Type.JSAPI.ToString())
            {
                resp.SetKey(WxJsApiPay.Key);
                appId = WxJsApiPay.AppId;
            }
            else if (TradeType == TenPayV3Type.APP.ToString())
            {
                resp.SetKey(WxAppPay.Key);
                appId = WxAppPay.AppId;
            }
        }

        #region 通知参数

        /// <summary>
        /// 返回状态码 SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string ReturnCode
        {
            get
            {
                return resp.GetParameter("return_code");
            }
        }

        /// <summary>
        /// 返回信息，如非空，为错误原因
        /// </summary>
        public string ReturnMsg
        {
            get
            {
                return resp.GetParameter("return_msg");
            }
        }


        public string AppId
        {
            get
            {
                return resp.GetParameter("appid");
            }
        }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo
        {
            get
            {
                return resp.GetParameter("out_trade_no");
            }
        }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string TransactionId
        {
            get
            {
                return resp.GetParameter("transaction_id");
            }
        }

        public string OpenId
        {
            get
            {
                return resp.GetParameter("openid");
            }
        }

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int TotalFee
        {
            get
            {
                return resp.GetParameter("total_fee").ToInt();
            }
        }

        /// <summary>
        /// 应结订单金额   =订单金额-非充值代金券金额，应结订单金额<=订单金额。
        /// </summary>
        public int SettlementTotalFee
        {
            get
            {
                return resp.GetParameter("settlement_total_fee").ToInt();
            }
        }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        public int CashFee
        {
            get
            {
                return resp.GetParameter("cash_fee").ToInt();
            }
        }

        /// <summary>
        /// 交易类型 	JSAPI、NATIVE、APP
        /// </summary>
        public string TradeType
        {
            get
            {
                return resp.GetParameter("trade_type");
            }
        }


        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string Attach
        {
            get
            {
                return resp.GetParameter("attach");
            }
        }

        /// <summary>
        /// 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string BankType
        {
            get
            {
                return resp.GetParameter("bank_type");
            }
        }

        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string FeeType
        {
            get
            {
                return resp.GetParameter("fee_type");
            }
        }

        /// <summary>
        /// 现金支付货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string CashFeeType
        {
            get
            {
                return resp.GetParameter("cash_fee_type");
            }
        }

        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string IsSubscribe
        {
            get
            {
                return resp.GetParameter("is_subscribe");
            }
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr
        {
            get
            {
                return resp.GetParameter("nonce_str");
            }
        }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign
        {
            get
            {
                return resp.GetParameter("sign");
            }
        }

        /// <summary>
        /// 签名类型 
        /// </summary>
        public string SignType
        {
            get
            {
                return resp.GetParameter("sign_type");
            }
        }

        /// <summary>
        /// 业务结果 SUCCESS/FAIL
        /// </summary>
        public string ResultCode
        {
            get
            {
                return resp.GetParameter("result_code");
            }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrCode
        {
            get
            {
                return resp.GetParameter("err_code");
            }
        }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string ErrCodeDes
        {
            get
            {
                return resp.GetParameter("err_code_des");
            }
        }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime TimeEnd
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (!resp.GetParameter("time_end").IsNullOrEmpty())
                {
                    try
                    {
                        result = DateTime.ParseExact(resp.GetParameter("time_end"), "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 总代金券金额 代金券金额<=订单金额，订单金额-代金券金额=现金支付金额
        /// </summary>
        public string CouponFee
        {
            get
            {
                return resp.GetParameter("coupon_fee");
            }
        }

        /// <summary>
        /// 代金券使用数量
        /// </summary>
        public int CouponCount
        {
            get
            {
                return resp.GetParameter("coupon_count").ToInt();
            }
        }

        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string DeviceInfo
        {
            get
            {
                return resp.GetParameter("device_info");
            }
        }

        /// <summary>
        /// 代金券类型  CASH-充值代金券 NO_CASH---非充值代金券
        /// coupon_type_$n  订单使用了免充值券后有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_0
        /// </summary>
        public string GetCouponType(int n)
        {
            return resp.GetParameter("coupon_type_" + n);
        }

        /// <summary>
        /// coupon_id_$n 代金券ID,$n为下标，从0开始编号
        /// </summary>
        public string GetCouponId(int n)
        {
            return resp.GetParameter("coupon_id_" + n);
        }

        /// <summary>
        /// coupon_fee_$n 单个代金券支付金额 $n为下标，从0开始编号
        /// </summary>
        public int GetCouponFee(int n)
        {
            return resp.GetParameter("coupon_fee_" + n).ToInt();
        }

        #endregion
    }
}
