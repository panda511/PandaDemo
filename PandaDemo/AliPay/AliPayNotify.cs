using Aop.Api.Util;
using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AliPay
{
    /// <summary>
    /// 支付宝回调通知类
    /// </summary>
    public class AliPayNotify
    {
        HttpRequest request = null;

        public AliPayNotify()
        {
            request = HttpContext.Current.Request;
        }

        /// <summary>
        /// 回调通知是否安全
        /// </summary>
        public bool IsSafe
        {
            get
            {
                bool safe = false;

                //验证商户
                if (this.AppId == AliPay.AppId && this.SellerId == AliPay.AppId)
                {
                    //获取当前请求中的post参数
                    var dict = new Dictionary<string, string>();
                    var keys = request.Form.AllKeys;
                    if (keys != null)
                    {
                        foreach (string key in keys)
                        {
                            dict.Add(key, request.Form[key]);
                        }
                    }

                    if (dict.Count > 0)
                    {
                        safe = AlipaySignature.RSACheckV1(dict, AliPay.AppPublicKey, AliPay.Charset);
                    }
                }

                return safe;
            }
        }

        #region 通知参数

        /// <summary>
        /// 通知时间
        /// </summary>
        public DateTime NotifyTime
        {
            get
            {
                return request.Form["notify_time"].ToDateTime();
            }
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        public string NotifyType
        {
            get
            {
                return request.Form["notify_type"];
            }
        }

        /// <summary>
        /// 通知校验ID
        /// </summary>
        public string NotifyId
        {
            get
            {
                return request.Form["notify_id"];
            }
        }

        /// <summary>
        /// 支付宝分配给开发者的应用Id
        /// </summary>
        public string AppId
        {
            get
            {
                return request.Form["app_id"];
            }
        }

        /// <summary>
        /// 编码格式，如utf-8、gbk、gb2312等
        /// </summary>
        public string Charset
        {
            get
            {
                return request.Form["charset"];
            }
        }

        /// <summary>
        /// 调用的接口版本
        /// </summary>
        public string Version
        {
            get
            {
                return request.Form["version"];
            }
        }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string SignType
        {
            get
            {
                return request.Form["sign_type"];
            }
        }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign
        {
            get
            {
                return request.Form["sign"];
            }
        }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string TradeNo
        {
            get
            {
                return request.Form["trade_no"];
            }
        }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo
        {
            get
            {
                return request.Form["out_trade_no"];
            }
        }

        /// <summary>
        /// 商户业务号，主要是退款通知中返回退款申请的流水号
        /// </summary>
        public string OutBizNo
        {
            get
            {
                return request.Form["out_biz_no"];
            }
        }

        /// <summary>
        /// 买家支付宝用户号
        /// </summary>
        public string BuyerId
        {
            get
            {
                return request.Form["buyer_id"];
            }
        }

        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string BuyerLogonId
        {
            get
            {
                return request.Form["buyer_logon_id"];
            }
        }

        /// <summary>
        /// 卖家支付宝用户号
        /// </summary>
        public string SellerId
        {
            get
            {
                return request.Form["seller_id"];
            }
        }

        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        public string SellerEmail
        {
            get
            {
                return request.Form["seller_email"];
            }
        }

        /// <summary>
        /// 交易状态 WAIT_BUYER_PAY 交易创建，等待买家付款， TRADE_CLOSED 未付款交易超时关闭，或支付完成后全额退款， TRADE_SUCCESS 交易支付成功， TRADE_FINISHED 交易结束，不可退款
        /// </summary>
        public string TradeStatus
        {
            get
            {
                return request.Form["trade_status"];
            }
        }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal TotalAmount
        {
            get
            {
                return request.Form["total_amount"].ToDecimal();
            }
        }

        /// <summary>
        /// 实收金额 商家在交易中实际收到的款项，单位为元
        /// </summary>
        public decimal ReceiptAmount
        {
            get
            {
                return request.Form["receipt_amount"].ToDecimal();
            }
        }

        /// <summary>
        /// 开票金额 用户在交易中支付的可开发票的金额
        /// </summary>
        public decimal InvoiceAmount
        {
            get
            {
                return request.Form["invoice_amount"].ToDecimal();
            }
        }

        /// <summary>
        /// 付款金额 用户在交易中支付的金额
        /// </summary>
        public decimal BuyerPayAmount
        {
            get
            {
                return request.Form["buyer_pay_amount"].ToDecimal();
            }
        }

        /// <summary>
        /// 集分宝金额 使用集分宝支付的金额
        /// </summary>
        public decimal PointAmount
        {
            get
            {
                return request.Form["point_amount"].ToDecimal();
            }
        }

        /// <summary>
        /// 总退款金额
        /// </summary>
        public decimal RefundFee
        {
            get
            {
                return request.Form["refund_fee"].ToDecimal();
            }
        }

        /// <summary>
        /// 订单标题
        /// </summary>
        public string Subject
        {
            get
            {
                return request.Form["subject"];
            }
        }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body
        {
            get
            {
                return request.Form["body"];
            }
        }

        /// <summary>
        /// 交易创建时间 该笔交易创建的时间。格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime GmtCreate
        {
            get
            {
                return request.Form["gmt_create"].ToDateTime();
            }
        }

        /// <summary>
        /// 交易付款时间 该笔交易的买家付款时间。格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime GmtPayment
        {
            get
            {
                return request.Form["gmt_payment"].ToDateTime();
            }
        }

        /// <summary>
        /// 交易退款时间 该笔交易的退款时间。格式为yyyy-MM-dd HH:mm:ss.S
        /// </summary>
        public DateTime GmtRefund
        {
            get
            {
                return request.Form["gmt_refund"].ToDateTime();
            }
        }

        /// <summary>
        /// 交易结束时间 该笔交易结束时间。格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime GmtClose
        {
            get
            {
                return request.Form["gmt_close"].ToDateTime();
            }
        }

        /// <summary>
        /// 支付金额信息 支付成功的各个渠道金额信息  [{“amount”:“15.00”,“fundChannel”:“ALIPAYACCOUNT”}]
        /// </summary>
        public string FundBillList
        {
            get
            {
                return request.Form["fund_bill_list"];
            }
        }

        /// <summary>
        /// 回传参数 公共回传参数，如果请求时传递了该参数，则返回给商户时会在异步通知时将该参数原样返回。本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>
        public string PassbackParams
        {
            get
            {
                return request.Form["passback_params"];
            }
        }

        /// <summary>
        /// 优惠券信息 本交易支付时所使用的所有优惠券信息 
        /// </summary>
        public string VoucherDetailList
        {
            get
            {
                return request.Form["voucher_detail_list"];
            }
        }

        #endregion
    }
}
