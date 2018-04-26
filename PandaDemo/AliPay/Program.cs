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
    class Program
    {
        static void Main(string[] args)
        {
            AliAppPay pay = new AliAppPay();
            decimal amount = 0.03m;
            string orderNo = "123456789";
            string subject = "iphone7 黑色 64G";
            string body = "京东商城"+ subject+ orderNo;
            string str = pay.GetPayParameter(amount, orderNo, subject, body);
            Console.WriteLine(str);
            Console.Read();
        }
    }

    public class AliAppPay
    {
        #region 支付配置项

        string appId = "2013092500031084";
        string notifyUrl = "http://www.qq.com/back.aspx";

        string appPrivateKey = @"
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
        string appPublicKey = @"
            MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDIgHnOn7LLILlKETd6BFRJ0Gq
            gS2Y3mn1wMQmyh9zEyWlz5p1zrahRahbXAfCfSqshSNfqOmAQzSHRVjCqjsAw1j
            yqrXaPdKBmr90DIpIxmIyKXv4GGAkPyJ/6FTFY99uhpiq0qadD/uSzQsefWo0aT
            vP/65zi3eof7TcZ32oWpwIDAQAB
        ";
        #endregion

        string url = "https://openapi.alipay.com/gateway.do";
        string format = "json";
        string version = "1.0";
        string signType = "RSA2";
        string charset = "utf-8";
        string productCode = "QUICK_MSECURITY_PAY";
        bool keyFromFile = false;

        /// <summary>
        /// 获取支付参数
        /// </summary>
        public string GetPayParameter(decimal amount, string orderNo, string subject, string body)
        {
            IAopClient client = new DefaultAopClient(url, appId, appPrivateKey, format, version, signType, appPublicKey, charset, keyFromFile);

            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();

            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel()
            {
                TotalAmount = amount.ToString(),
                OutTradeNo = orderNo,
                Subject = subject,
                Body = body,
                ProductCode = productCode,
            };

            request.SetBizModel(model);
            request.SetNotifyUrl(notifyUrl);

            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);

            //response.Body就是orderString 可以直接给客户端请求，无需再做处理。
            return response.Body;
        }
    }
}
