using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Extension;
using Extension.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AliPay
{
    /// <summary>
    /// 支付宝支付类
    /// </summary>
    public class AliPay
    {
        #region 支付配置项

        public static readonly string AppId = "2013092500031084"; //ConfigurationManager.AppSettings["AliPay_AppId"]
        public static readonly string NotifyUrl = "http://www.qq.com/back.aspx";

        public static readonly string QuitUrl = "http://www.qq.com/back.aspx"; //支付中途退出返回商户网站地址(web支付用)
        public static readonly string ReturnUrl = "http://www.qq.com/back.aspx"; //支付完成同步回调地址(web支付用)

        #region key
        public static readonly string AppPrivateKey = @"
            MIICXQIBAAKBgQDIgHnOn7LLILlKETd6BFRJ0GqgS2Y3mn1wMQmyh9zEyWlz5p1z
            rahRahbXAfCfSqshSNfqOmAQzSHRVjCqjsAw1jyqrXaPdKBmr90DIpIxmIyKXv4G
            GAkPyJ/6FTFY99uhpiq0qadD/uSzQsefWo0aTvP/65zi3eof7TcZ32oWpwIDAQAB
            AoGBALrKLjBXyRrCFryxA2zyIZBO0TcaZ1T/4UKm/LDNL9hJB6wJOcBuFTQb0MFn
            tkLALmOo2DYHQj4EzS+Xy2jp6pMsvCKG63XbU/7d7w1/ejTYxz+MX4ZzG6Ro5Cmb
            pY1inbithQIfN3noK0h+PE7lkvOy43mTR4a+ceAtCh3gU2HpAkEA+XMqxbirmuhj
            RtHU4+YAOu0fY3JrXA1oErLiF0tJc1HhsWQuMv8v++peku4wWLhpM/8dpiWmf29z
            McXBkNNWDQJBAM3ERi014EzIN91Dkdd6Kl4lfXEG62h0f2j5kvJGcZCTWKMLNyt7
            pjVYy4RRrrLLZiyGNqSKTzVDoaNWGGrTxoMCQQDavL54+uKfx7+mTkGcRgdVpLCt
            h5vU8HyeSPYw7vfNg7Og1fQdC+CLyox70xnZ8ntt+PuKweEqRhSBRKPj1y3RAkAY
            jjzFtnE/GIG6MQ8dhOG7fIPc0jOTsptl3qrPqOJym3Lvei4qTUZHhYI8Fzde9PEL
            jTTGLA9JzvliMasWTJGbAkB7UviY/ywp0GYCysV2QJZcakk5QZZ9mJwYJOh6hy6f
            BJJ8dq1F4aMRNbw/bTZqJuj4mmZUMrpK0JKeRjjFgzJF
        ";
        public static readonly string AppPublicKey = @"
            MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDIgHnOn7LLILlKETd6BFRJ0Gq
            gS2Y3mn1wMQmyh9zEyWlz5p1zrahRahbXAfCfSqshSNfqOmAQzSHRVjCqjsAw1j
            yqrXaPdKBmr90DIpIxmIyKXv4GGAkPyJ/6FTFY99uhpiq0qadD/uSzQsefWo0aT
            vP/65zi3eof7TcZ32oWpwIDAQAB
        ";
        #endregion

        public const string Gateway = "https://openapi.alipay.com/gateway.do";
        public const string Format = "json";
        public const string Version = "1.0";
        public const string SignType = "RSA2";
        public const string Charset = "utf-8";
        public const string ProductCode = "QUICK_MSECURITY_PAY";

        #endregion

        IAopClient aopClient = new DefaultAopClient(Gateway, AppId, AppPrivateKey, Format, Version, SignType, AppPublicKey, Charset, false);

        #region 支付

        /// <summary>
        /// 获取APP支付参数
        /// </summary>
        public string GetAppPayParameter(decimal amount, string orderNo, string subject, string body, string passbackParams = null)
        {
            string parameter = string.Empty;

            //组装业务参数
            var model = new AlipayTradeAppPayModel()
            {
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                Subject = subject,
                Body = body,
                ProductCode = ProductCode,
                PassbackParams = HttpUtility.UrlEncode(passbackParams)
            };

            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            var request = new AlipayTradeAppPayRequest();
            request.SetBizModel(model);
            request.SetNotifyUrl(NotifyUrl);

            AlipayTradeAppPayResponse response = aopClient.SdkExecute(request);
            if (response != null && response.Code == "10000")
            {
                parameter = response.Body;
            }

            //记日志response.ToJson();

            return parameter;
        }

        /// <summary>
        /// 获取Web支付参数(一个from表单)
        /// </summary>
        public string GetWebPayParameter(decimal amount, string orderNo, string subject, string body, string passbackParams = null)
        {
            string parameter = string.Empty; 

            //组装业务参数
            var model = new AlipayTradeWapPayModel()
            {
                Body = body,
                Subject = subject,
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                ProductCode = ProductCode,
                QuitUrl = QuitUrl,
                PassbackParams = HttpUtility.UrlEncode(passbackParams)
            };

            var request = new AlipayTradeWapPayRequest();
            request.SetBizModel(model);
            request.SetReturnUrl(ReturnUrl); // 支付中途退出返回商户网站地址
            request.SetNotifyUrl(NotifyUrl); // 设置支付完成异步通知接收地址

            AlipayTradeWapPayResponse response = aopClient.pageExecute(request, null, "post");
            if (response != null && response.Code == "10000")
            {
                parameter = response.Body;
            }

            //记日志response.ToJson();

            return parameter;
        }

        /// <summary>
        /// 获取扫码支付的二维码图片链接(将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。)
        /// </summary>
        public string GetQrCodePayLink(decimal amount, string orderNo, string subject, string body, string storeId, string operatorId)
        {
            string link = string.Empty;

            //组装业务参数
            var model = new AlipayTradePrecreateModel()
            {
                Body = body,
                Subject = subject,
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                StoreId = storeId, //商户门店编号
                OperatorId = operatorId
            };

            var request = new AlipayTradePrecreateRequest();
            request.SetBizModel(model);

            //不推荐使用异步通知,避免单边账问题发生。推荐使用轮询撤销机制
            //对所有唤起收银台的交易（支付宝返回状态10003(等待用户支付)）发起轮询，并且建议轮询总时间设为30秒，轮询平均间隔设为3秒。在让用户再次支付前，必须通过查询确认当前订单的状态。
            //request.SetNotifyUrl(NotifyUrl);

            AlipayTradePrecreateResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                link = response.QrCode;
            }

            //记日志response.ToJson();

            return link;
        }


        /// <summary>
        /// 条码支付
        /// </summary>
        public void BarCodePay(decimal amount, string orderNo, string authCode, string subject, string storeId, string terminalId)
        {
            bool success = false;

            //组装业务参数
            var model = new AlipayTradePayModel()
            {
                OutTradeNo = orderNo,
                TotalAmount = amount.ToString(),
                Scene = "bar_code",
                AuthCode = authCode,
                Subject = subject,
                StoreId = storeId,
                TerminalId = terminalId,
                TimeoutExpress = "2m"
            };

            var request = new AlipayTradePayRequest();
            request.SetBizModel(model);

            AlipayTradePayResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                success = true;

                //response.TradeNo 支付宝28位交易号
            }

            //记日志response.ToJson();

            //return success;
        }

        #endregion



        /// <summary>
        /// 退款
        /// </summary>
        public bool Refund(decimal amount, string orderNo, string requestNo, string reason = null, string tradeNo = null)
        {
            bool success = false;

            //组装业务参数
            var model = new AlipayTradeRefundModel()
            {
                OutTradeNo = orderNo,
                TradeNo = tradeNo,
                RefundAmount = amount.ToString(),
                RefundReason = reason,
                OutRequestNo = requestNo //退款单号，同一笔多次退款需要保证唯一，部分退款该参数必填。
            };

            var request = new AlipayTradeRefundRequest();
            request.SetBizModel(model);

            AlipayTradeRefundResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                success = true;
            }

            //记日志response.ToJson();

            return success;
        }


        /// <summary>
        /// 交易撤销
        /// </summary>
        public bool Cancel(string orderNo, string tradeNo = null)
        {
            bool success = false;

            var model = new AlipayTradeCancelModel()
            {
                OutTradeNo = orderNo,
                TradeNo = tradeNo
            };

            var request = new AlipayTradeCancelRequest();
            request.SetBizModel(model);

            AlipayTradeCancelResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                success = true;
                //response.RetryFlag 是否需要重试，Y / N
                //response.Action 本次撤销触发的交易动作 close：关闭交易，无退款； refund：产生了退款
            }

            return success;
        }



        /// <summary>
        /// 订单是否支付成功
        /// </summary>
        public bool IsPaySuccess(string orderNo, decimal amount)
        {
            bool success = false;

            //组装业务参数
            var model = new AlipayTradeQueryModel()
            {
                OutTradeNo = orderNo
            };

            var request = new AlipayTradeQueryRequest();
            request.SetBizModel(model);

            AlipayTradeQueryResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                if (response.TradeStatus == AliPayTradeStatus.TRADE_SUCCESS.ToString() || response.TradeStatus == AliPayTradeStatus.TRADE_FINISHED.ToString())
                {
                    if (response.TotalAmount.ToDecimal() == amount)
                    {
                        success = true;
                    }
                }
            }

            //记日志response.ToJson();

            return success;
        }


        /// <summary>
        /// 获取某天的对账单的下载地址(商户可通过接口下载指定日期（当天除外）的业务明细账单文件，并结合自身业务系统实现自动对账。该下载链接仅30秒，在得到链接后系统需要立刻请求下载账单文件。)
        /// </summary>
        public string GetBillDownloadUrl(DateTime date)
        {
            string url = string.Empty;

            //组装业务参数
            var model = new AlipayDataDataserviceBillDownloadurlQueryModel()
            {
                BillType = "trade",
                BillDate = date.ToString("yyyy-MM-dd")
            };

            var request = new AlipayDataDataserviceBillDownloadurlQueryRequest();
            request.SetBizModel(model);

            AlipayDataDataserviceBillDownloadurlQueryResponse response = aopClient.Execute(request);
            if (response != null && response.Code == "10000")
            {
                url = response.BillDownloadUrl;
            }

            //记日志response.ToJson();

            return url;
        }

    }
}
