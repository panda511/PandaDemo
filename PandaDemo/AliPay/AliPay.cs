using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 获取APP支付参数
        /// </summary>
        public string GetAppPayParameter(decimal amount, string orderNo, string subject, string body, string passbackParams = null)
        {
            IAopClient client = new DefaultAopClient(Gateway, AppId, AppPrivateKey, Format, Version, SignType, AppPublicKey, Charset, false);

            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel()
            {
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                Subject = subject,
                Body = body,
                ProductCode = ProductCode,
                PassbackParams = passbackParams
            };

            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            request.SetBizModel(model);
            request.SetNotifyUrl(NotifyUrl);

            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            return response.Body;
        }

        /// <summary>
        /// 获取Web支付参数(一个from表单)
        /// </summary>
        public string GetWebPayParameter(decimal amount, string orderNo, string subject, string body, string passbackParams = null)
        {
            IAopClient client = new DefaultAopClient(Gateway, AppId, AppPrivateKey, Format, Version, SignType, AppPublicKey, Charset, false);

            //组装业务参数
            AlipayTradeWapPayModel model = new AlipayTradeWapPayModel()
            {
                Body = body,
                Subject = subject,
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                ProductCode = ProductCode,
                QuitUrl = QuitUrl,
                PassbackParams = passbackParams
            };

            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            request.SetBizModel(model);
            request.SetReturnUrl(ReturnUrl); // 支付中途退出返回商户网站地址
            request.SetNotifyUrl(NotifyUrl); // 设置支付完成异步通知接收地址

            AlipayTradeWapPayResponse response = client.pageExecute(request, null, "post");
            return response.Body;
        }
    }
}
