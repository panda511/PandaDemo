using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public interface IWxPay
    {
        WxPayParameter GetPayParameter(string orderNo, int amout, string body, string ip, string openId);
    }
}
