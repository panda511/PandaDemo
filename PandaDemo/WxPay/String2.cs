using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay
{
    public class String2
    {
        public static string GetGuid(string format = "N")
        {
            return Guid.NewGuid().ToString("N");
        }
    }

    public class DateTime2
    {
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
