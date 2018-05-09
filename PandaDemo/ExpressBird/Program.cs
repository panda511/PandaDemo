using Extension;
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
            //bird.GetEOrder();
        }
    }


    /// <summary>
    /// 电子面单寄(收)件人
    /// </summary>
    public class EOrderPerson
    {
        /// <summary>
        /// 寄(收)件人公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 寄(收)件人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 寄(收)件人手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 寄(收)件人电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string ExpAreaName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
    }

    /// <summary>
    /// 电子面单物品
    /// </summary>
    public class EOrderCommodity
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Goodsquantity { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal GoodsPrice { get; set; }

        /// <summary>
        /// 商品重量kg
        /// </summary>
        public decimal GoodsWeight { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string GoodsDesc { get; set; }

        /// <summary>
        /// 商品体积m3
        /// </summary>
        public decimal GoodsVol { get; set; }
    }


    /// <summary>
    /// 快递鸟处理类
    /// </summary>
    public class KuaiDiNiao
    {
        public static readonly string EBusinessID = ""; //电商ID
        public static readonly string AppKey = "";      //电商加密私钥

        private string domain = "http://api.kdniao.cc/api/";

        /// <summary>
        /// 获取电子面单
        /// </summary>
        /// <param name="orderCode">订单编号</param>
        /// <param name="cost">寄件费（运费）</param>
        /// <param name="shipperCode">快递公司编码</param>
        /// <param name="customerName">电子面单客户账号（与快递网点申请）</param>
        /// <param name="customerPwd">电子面单密码</param>
        /// <param name="sendSite">收件网点标识</param>
        /// <param name="monthCode">月结编码</param>
        /// <param name="otherCost">其他费用</param>
        public EOrderResponse GetEOrder(string orderCode, string shipperCode, string customerName, string customerPwd, string sendSite, string monthCode, int payType, EOrderPerson sender, EOrderPerson receiver, List<EOrderCommodity> commodityList, decimal cost = 0, decimal otherCost = 0, string logisticCode = "", int expType = 1, int isNotice = 1, string callBack = "", string thrOrderCode = "", decimal weight = 0, int quantity = 0, decimal volume = 0, string remark = "", int isReturnPrintTemplate = 0, int isSendMessage = 0, string templateSize = "", string operateRequire = "")
        {
            var data = new
            {
                #region 必传参数

                MemberID = EBusinessID, //会员标识
                CustomerName = customerName, //电子面单客户账号（与快递网点申请）
                CustomerPwd = customerPwd, //电子面单密码
                SendSite = sendSite, //收件网点标识
                MonthCode = monthCode,//月结编码
                ShipperCode = shipperCode, //快递公司编码
                OrderCode = orderCode, //订单编号
                PayType = payType, //运费支付方式:1-现付，2-到付，3-月结，4-第三方支付

                Commodity = commodityList, //物品信息

                #region 发件人信息
                Sender = new
                {
                    sender.Company,
                    sender.Name,
                    sender.Mobile,
                    sender.PostCode,
                    sender.ProvinceName,
                    sender.CityName,
                    sender.ExpAreaName,
                    sender.Address
                },
                #endregion

                #region 收件人信息
                Receiver = new
                {
                    receiver.Company,
                    receiver.Name,
                    receiver.Mobile,
                    receiver.PostCode,
                    receiver.ProvinceName,
                    receiver.CityName,
                    receiver.ExpAreaName,
                    receiver.Address
                },
                #endregion

                #endregion

                #region 选传参数

                Cost = cost, //寄件费（运费）
                OtherCost = otherCost, //其他费用
                LogisticCode = logisticCode, //快递单号
                ExpType = expType,//快递类型：1-标准快件
                IsNotice = isNotice, //是否通知快递员上门揽件：0-通知；1-不通知；不填则默认为1
                CallBack = callBack, //用户自定义回调信息
                ThrOrderCode = thrOrderCode, //第三方订单号 (ShipperCode 为 JD 且 ExpType 为 1 时必填) 
                Weight = weight, //包裹总重量 kg 
                Quantity = quantity, //包裹数，一个包裹对应一个 运单号，如果是大于 1 个包 裹，返回则按照子母件的方 式返回母运单号和子运单号 
                Volume = volume, //包裹总体积 m3 
                Remark = remark,
                IsReturnPrintTemplate = isReturnPrintTemplate, //返回电子面单模板：0-不需要；1-需要
                IsSendMessage = isSendMessage, //是否订阅短信：0-不需要；1-需要
                TemplateSize = templateSize, //模板规格(默认的模板无需传值，非默认模板传对应模板尺寸)
                OperateRequire = operateRequire //操作要求(如：签名、盖章、 身份证复印件等) 

                //StartDate = "", //上门取货时间段 开始 yyyy-MM-dd HH:mm:ss
                //EndDate = "", //上门取货时间段 结束 yyyy-MM-dd HH:mm:ss

                #region 增值服务
                //AddService = new List<object>
                //{
                //    new
                //    {
                //        Name = "", //增值服务名称
                //        Value = "", //增值服务值
                //        CustomerID = "" //客户标识（选填）
                //    },
                //    new
                //    {
                //       Name = "", //增值服务名称
                //       Value = "", //增值服务值
                //       CustomerID = "" //客户标识（选填）
                //    }
                //},
                #endregion

                #endregion
            };

            string param = data.ToJson();
            string sign = (param + AppKey).ToMd5().ToBase64();

            //请求参数
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "EBusinessID", EBusinessID },
                { "RequestType", "1007" }, //请求指令类型
                { "DataType", "2" }, //JSON格式
                { "RequestData", HttpUtility.UrlEncode(param, Encoding.UTF8) },
                { "DataSign", HttpUtility.UrlEncode(sign, Encoding.UTF8)}
            };

            string result = PostEOrderService(dict);
            return result.ToObject<EOrderResponse>();
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

            string url = domain + "Eorderservice";
            url = "http://sandboxapi.kdniao.cc:8080/kdniaosandbox/gateway/exterfaceInvoke.json"; //测试地址

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
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
