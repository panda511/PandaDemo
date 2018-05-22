using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCommon.Extention
{
    public static class IntExtention
    {
        public static bool ToBool(this int value)
        {
            bool result = false;
            if (value > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
