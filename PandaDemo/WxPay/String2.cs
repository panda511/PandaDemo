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

   
}
