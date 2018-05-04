﻿using Extension;
using Extension.Extention;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExpressBird
{
    /*  {
            "EBusinessID": "1237100",
            "ResultCode": "105",
            "Reason": "订单号已存在，请勿重复操作"，
            "UniquerRequestNumber":"5e66486b-8fbc-4131-b875-9b13d2ad1354"
        }
    * 
    *        
    *        
        {
            "EBusinessID": "1237100",
            "Order": {
                "OrderCode": "012657700387",
                "ShipperCode": "HTKY",
                "LogisticCode": "50002498503427",
                "MarkDestination": "京-朝阳(京-1)",
                "OriginCode": "200000",
                "OriginName": "上海分拨中心",
                "PackageCode": "北京"
            },
            "PrintTemplate":"此处省略打印模板HTML内容",
            "EstimatedDeliveryTime":"2016-03-06",
            "Callback":"调用时传入的Callback",
            "Success": true,
            "ResultCode": "100",
            "Reason": "成功"
        }
    * 
    * ***/


    class Program
    {
        static void Main(string[] args)
        {
            KuaiDiNiao bird = new KuaiDiNiao();
            //bird.CreateOrder();
        }
    }




    public class KuaiDiNiao
    {
        public static readonly string EBusinessID = ""; //电商ID
        public static readonly string AppKey = "";      //电商加密私钥

        public EOrderResponse GetEOrder()
        {
            string requestType = "1007"; //请求指令类型
            string dataType = "2"; //JSON格式

            string requestData = GetRequestData("");

            //请求参数
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("EBusinessID", EBusinessID);
            param.Add("RequestType", requestType);
            param.Add("DataType", dataType);

            param.Add("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8));

            string dataSign = (requestData + AppKey).ToMd5().ToBase64();
            param.Add("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));

            string result = PostEOrderService(param);
            return result.ToObject<EOrderResponse>();
        }


        string GetRequestData(string orderCode, decimal cost, string shipperCode, string customerName, string customerPwd, string sendSite, string monthCode, decimal otherCost=0)
        {
            var RequestData = new
            {
                CallBack = "", //用户自定义回调信息
                MemberID = "", //会员标识


                CustomerName = customerName, //电子面单客户账号（与快递网点申请）
                CustomerPwd = customerPwd, //电子面单密码
                SendSite = sendSite, //收件网点标识


                ShipperCode = shipperCode, //快递公司编码
                LogisticCode = "", //快递单号


                OrderCode = orderCode, //订单编号
                ThrOrderCode = "", //第三方订单编号

                MonthCode = monthCode,//月结编码


                PayType = 1, //邮费支付方式:1-现付，2-到付，3-月结，4-第三方支付
                ExpType = 1,//快递类型：1-标准快件
                IsNotice = 1, //是否通知快递员上门揽件：0-通知；1-不通知；不填则默认为1

                Cost = cost, //寄件费（运费）
                OtherCost = otherCost, //其他费用

                //发件人信息
                Sender = new
                {
                    Company = "", //发件人公司
                    Name = "", //发件人
                    Mobile = "",
                    PostCode = "",//发件人邮编
                    ProvinceName = "江苏省",
                    CityName = "苏州市",
                    ExpAreaName = "姑苏区",
                    Address = "人民路12号"
                },

                //收件人信息
                Receiver = new
                {
                    Company = "", //收件人公司
                    Name = "", //收件人
                    Mobile = "",
                    PostCode = "",//发件人邮编
                    ProvinceName = "江苏省",
                    CityName = "苏州市",
                    ExpAreaName = "姑苏区",
                    Address = "人民路12号"
                },

                StartDate = "yyyy-MM-dd HH:mm:ss", //上门取货时间段 开始
                EndDate = "yyyy-MM-dd HH:mm:ss", //上门取货时间段 结束

                Weight = 65.24, //物品总重量kg
                Quantity = 1, //件数/包裹数
                Volume = 6.98, //物品总体积m3
                Remark = "小心轻放",

                AddService = new
                {
                    Name = "", //增值服务名称
                    Value = "", //增值服务值
                    CustomerID = "" //客户标识（选填）
                },

                //物品
                Commodity = new List<object>() {
                    new {
                        GoodsName="", //商品名称
                        GoodsCode="", //商品编码
                        Goodsquantity=1, //商品数量
                        GoodsPrice=21.28, //商品价格
                        GoodsWeight=1.36, //商品重量kg
                        GoodsDesc="", //商品描述
                        GoodsVol=54.32, //商品体积m3
                    },
                     new {
                        GoodsName="", //商品名称
                        GoodsCode="", //商品编码
                        Goodsquantity=1, //商品数量
                        GoodsPrice=21.28, //商品价格
                        GoodsWeight=1.36, //商品重量kg
                        GoodsDesc="", //商品描述
                        GoodsVol=54.32, //商品体积m3
                    }
                },

                IsReturnPrintTemplate = 1, //返回电子面单模板：0-不需要；1-需要
                IsSendMessage = 0, //是否订阅短信：0-不需要；1-需要
                TemplateSize = "", //模板规格(默认的模板无需传值，非默认模板传对应模板尺寸)
            };

            return RequestData.ToJson();
        }



       /// <summary>
       /// 请求电子面单接口
       /// </summary>
        private string PostEOrderService(Dictionary<string, string> param)
        {
            string result = string.Empty; ;

            StringBuilder postData = new StringBuilder();
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    if (postData.Length > 0)
                    {
                        postData.Append("&");
                    }
                    postData.Append(p.Key);
                    postData.Append("=");
                    postData.Append(p.Value);
                }
            }

            byte[] byteData = Encoding.UTF8.GetBytes(postData.ToString());

            string url = "http://api.kdniao.cc/api/Eorderservice";
            //url = "http://sandboxapi.kdniao.cc:8080/kdniaosandbox/gateway/exterfaceInvoke.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = url;
            request.Accept = "*/*";
            request.Timeout = 30 * 1000;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.Method = "POST";
            request.ContentLength = byteData.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
                stream.Flush();
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream backStream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(backStream, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                    backStream.Close();
                }
            }

            response.Close();
            request.Abort();

            return result;
        }
    }


    /// <summary>
    /// 电子面单接口返回
    /// </summary>
    public class EOrderResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string EBusinessID { get; set; }

        /// <summary>
        /// 电子面单
        /// </summary>
        public EOrder Order { get; set; }

        /// <summary>
        /// 成功与否
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniquerRequestNumber { get; set; }

        /// <summary>
        /// 	面单打印模板
        /// </summary>
        public string PrintTemplate { get; set; }

        /// <summary>
        /// 订单预计到货时间yyyy-mm-dd
        /// </summary>
        public string EstimatedDeliveryTime { get; set; }

        /// <summary>
        /// 用户自定义回调信息
        /// </summary>
        public string Callback { get; set; }

        /// <summary>
        /// 子单数量
        /// </summary>
        public int SubCount { get; set; }

        /// <summary>
        /// 子单号
        /// </summary>
        public string SubOrders { get; set; }

        /// <summary>
        /// 子单模板
        /// </summary>
        public string SubPrintTemplates { get; set; }
    }

    /// <summary>
    /// 电子面单
    /// </summary>
    public class EOrder
    {
        public string OrderCode { get; set; }

        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string LogisticCode { get; set; }

        /// <summary>
        /// 大头笔
        /// </summary>
        public string MarkDestination { get; set; }

        /// <summary>
        /// 始发地区域编码
        /// </summary>
        public string OriginCode { get; set; }

        /// <summary>
        /// 始发地/始发网点
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 目的地区域编码 
        /// </summary>
        public string DestinatioCode { get; set; }

        /// <summary>
        /// 目的地/到达网点
        /// </summary>
        public string DestinatioName { get; set; }

        /// <summary>
        /// 分拣编码
        /// </summary>
        public string SortingCode { get; set; }

        /// <summary>
        /// 集包编码
        /// </summary>
        public string PackageCode { get; set; }
    }




}