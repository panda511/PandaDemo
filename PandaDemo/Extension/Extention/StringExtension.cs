using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static string TrimEnd2(this string value, params char[] trimChars)
        {
            string result = value;
            if (!value.IsNullOrEmpty())
            {
                result = value.TrimEnd(trimChars);
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



        public static string ToSHA1(this string value)
        {
            string result = string.Empty;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] array = sha1.ComputeHash(Encoding.Unicode.GetBytes(value));
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].ToString("x2");
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

        public static bool ToBool(this string value)
        {
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }

        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result == DateTime.MinValue ? defaultValue : result;
        }

        public static DateTime ToDateTime(this string value)
        {
            return ToDateTime(value, DateTime.MinValue);
        }


        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }



    }
}
