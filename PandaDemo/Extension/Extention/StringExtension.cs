using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string Trim2(this string value)
        {
            string result = value;
            if (!value.IsNullOrEmpty())
            {
                result = value.Trim();
            }
            return result;
        }

        public static string PadLeft2(this string value, int totalWidth, char paddingChar)
        {
            string result = string.Empty;
            if (!value.IsNullOrEmpty())
            {
                result = value;
            }
            return result.PadLeft(totalWidth, paddingChar);
        }

        public static int GetLength(this string value)
        {
            int result = 0;
            if (!value.IsNullOrEmpty())
            {
                result = value.Length;
            }
            return result;
        }








        public static int ToInt(this string value, int defaultValue = 0)
        {
            int result = 0;
            bool success = Int32.TryParse(value, out result);
            if (!success)
            {
                result = defaultValue;
            }
            return result;
        }


        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }



    }
}
