using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliPay
{
    /// <summary>
    /// 支付宝交易状态
    /// </summary>
    public enum AliPayTradeStatus
    {
        /// <summary>
        /// 交易创建，等待买家付款（不触发通知）
        /// </summary>
        WAIT_BUYER_PAY,

        /// <summary>
        /// 交易支付成功（触发通知）
        /// </summary>
        TRADE_SUCCESS,

        /// <summary>
        /// 未付款交易超时关闭，或支付完成后全额退款（触发通知）
        /// </summary>
        TRADE_CLOSED,

        /// <summary>
        /// 交易结束，不可退款（触发通知）
        /// </summary>
        TRADE_FINISHED
    }
}
