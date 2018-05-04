using Extension;
using Extension.Extention;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShouQianBa
{
    class Program
    {
        static void Main(string[] args)
        {
            ShouQianBa sqb = new ShouQianBa("zaxscd", "123edsw");
            var resp = sqb.Pay("asd", "zxc", "123", 1, "subject", "jim");
            Console.WriteLine(resp.ToJson());
            Console.Read();
        }
    }

    public class ShouQianBa
    {
        /// <summary>
        /// 收钱吧终端ID
        /// </summary>
        public string TerminalSn { get; }

        /// <summary>
        /// 终端密钥，支付类接口使用终端序列号和终端密钥进行签名
        /// </summary>
        public string TerminalKey { get; }

        public ShouQianBa(string terminalSn, string terminalKey)
        {
            TerminalSn = terminalSn;
            TerminalKey = terminalKey;
        }

        /// <summary>
        /// 收钱吧支付
        /// </summary>
        /// <param name="deviceId">设备指纹</param>
        /// <param name="dynamicId">条码内容</param>
        /// <param name="orderNo">商户系统订单号</param>
        /// <param name="amount">交易总金额 单位分</param>
        /// <param name="subject">交易简介</param>
        /// <param name="operatePerson">发起本次交易的操作员</param>
        /// <param name="description">商品详情</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="extended">扩展参数集合</param>
        /// <param name="reflect">反射参数 任何调用者希望原样返回的信息</param>
        /// <param name="notifyUrl">支付回调的地址</param>
        /// <param name="payWay">支付方式 1:支付宝 3:微信 4:百度钱包 5:京东钱包 6:qq钱包</param>
        /// <returns></returns>
        public PayResponse Pay(string deviceId, string dynamicId, string orderNo, int amount, string subject, string operatePerson, string description = "", string longitude = "", string latitude = "", string extended = "", string reflect = "", string notifyUrl = "", string payWay = "")
        {
            #region 支付请求参数

            JObject jObject = new JObject();
            jObject.Add(new JProperty("terminal_sn", TerminalSn));
            jObject.Add(new JProperty("client_sn", orderNo));
            jObject.Add(new JProperty("total_amount", amount.ToString()));
            jObject.Add(new JProperty("dynamic_id", dynamicId));
            jObject.Add(new JProperty("subject", subject));
            jObject.Add(new JProperty("operator", operatePerson));
            jObject.Add(new JProperty("device_id", deviceId));
            jObject.Add(new JProperty("description", description));
            jObject.Add(new JProperty("longitude", longitude));
            jObject.Add(new JProperty("latitude", latitude));
            jObject.Add(new JProperty("extended", extended));
            jObject.Add(new JProperty("reflect", reflect));
            jObject.Add(new JProperty("notify_url", notifyUrl));
            jObject.Add(new JProperty("payway", payWay));

            var data = new
            {
                terminal_sn = TerminalSn,
                client_sn = orderNo,
                total_amount = amount,
                dynamic_id = dynamicId,
                subject,
                Operator = operatePerson,
                device_id = deviceId,
                description,
                longitude,
                latitude,
                extended,
                reflect,
                notify_url = notifyUrl,
                payway = payWay
            };


            string param = jObject.ToString(); //data.ToJson();
            string sign = (param + TerminalKey).ToMd5();

            #endregion

            string resp = PostPay(param, sign);
            return resp.ToObject<PayResponse>();
        }

        /// <summary>
        /// 请求支付接口
        /// </summary>
        private string PostPay(string body, string sign)
        {
            Encoding encoding = Encoding.UTF8;

            string url = "https://api.shouqianba.com/upay/v2/pay";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, TerminalSn + " " + sign);

            byte[] bytes = encoding.GetBytes(body);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }

            //发送成功后接收返回的json信息
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream newStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(newStream, encoding);
            return streamReader.ReadToEnd();
        }
    }

    public class PayResponse
    {
        /// <summary>
        /// 200代表支付成功
        /// </summary>
        public string Result_Code { get; set; }

        public string Error_Code { get; set; }

        public string Error_Message { get; set; }

        public BizResponse Biz_Response { get; set; }
    }

    public class BizResponse
    {
        /// <summary>
        /// 结果码 表示接口调用的业务逻辑是否成功 PAY_SUCCESS
        /// </summary>
        public string Result_Code { get; set; }

        /// <summary>
        /// 错误码  INVALID_BARCODE
        /// </summary>
        public string Error_Code { get; set; }

        /// <summary>
        /// 错误消息 不合法的支付条码
        /// </summary>
        public string Error_Message { get; set; }

        public PayData Data { get; set; }
    }

    public class PayData
    {
        /// <summary>
        /// 收钱吧系统内部唯一订单号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 商户系统订单号
        /// </summary>
        public string Client_Sn { get; set; }

        /// <summary>
        /// 本次操作产生的流水的状态 SUCCESS
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 一级支付方式，取值见附录《支付方式列表》 1支付宝 2支付宝 3微信 4百度钱包 5京东 6qq钱包
        /// </summary>
        public int PayWay { get; set; }

        /// <summary>
        /// 二级支付方式，取值见附录《二级支付方式列表》 1条码支付 2二维码支付 3wap支付 4小程序支付
        /// </summary>
        public int Sub_PayWay { get; set; }

        /// <summary>
        /// 订单状态 PAID
        /// </summary>
        public string Order_Status { get; set; }

        /// <summary>
        /// 支付平台上(微信，支付宝)的付款人账号
        /// </summary>
        public string Payer_Login { get; set; }

        /// <summary>
        /// 支付平台（微信，支付宝）上的付款人ID
        /// </summary>
        public string Payer_Uid { get; set; }

        /// <summary>
        /// 支付通道交易凭证号
        /// </summary>
        public string Trade_No { get; set; }

        /// <summary>
        /// 本次交易总金额
        /// </summary>
        public int Total_Amount { get; set; }

        /// <summary>
        /// 实收金额 如果没有退款，这个字段等于total_amount。否则等于 total_amount减去退款金额
        /// </summary>
        public int Net_Amount { get; set; }

        /// <summary>
        /// 付款动作在收钱吧的完成时间 时间戳 1449646835244
        /// </summary>
        public string Finish_Time { get; set; }

        /// <summary>
        /// 付款动作在支付服务商的完成时间 时间戳 1449646835244
        /// </summary>
        public string Channel_Finish_Time { get; set; }

        /// <summary>
        /// 本次交易概述
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 反射参数
        /// </summary>
        public string Reflect { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 活动优惠
        /// </summary>
        public List<Payment> Payment_List { get; set; }

        public string Store_Id { get; set; }
        public string Client_tsn { get; set; }

        public string Ctime { get; set; }

    }

    /// <summary>
    /// 活动优惠
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public string Amount_Total { get; set; }
    }
}
