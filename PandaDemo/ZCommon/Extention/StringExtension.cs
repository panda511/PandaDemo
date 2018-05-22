using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZCommon.Extention
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        #region AvoidNull

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

        public static int Length2(this string value)
        {
            int result = 0;
            if (!value.IsNullOrEmpty())
            {
                result = value.Length;
            }
            return result;
        }

        #endregion

        #region Encode

        public static string ToBase64(this string value, Encoding encoding = null)
        {
            Encoding encoding2 = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding2.GetBytes(value);
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        public static string FromBase64(this string value, Encoding encoding = null)
        {
            Encoding encoding2 = encoding ?? Encoding.UTF8;
            byte[] bytes = Convert.FromBase64String(value);
            string result = encoding2.GetString(bytes);
            return result;
        }

        public static string ToSha1(this string value, Encoding encoding = null)
        {
            string result = string.Empty;
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            Encoding encoding2 = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding2.GetBytes(value);
            byte[] hash = sha1.ComputeHash(bytes);

            for (int i = 0; i < hash.Length; i++)
            {
                result += hash[i].ToString("x2");
            }
            return result;
        }

        public static string ToMd5(this string value, Encoding encoding = null)
        {
            string result = string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();

            Encoding encoding2 = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding2.GetBytes(value);
            byte[] hash = md5.ComputeHash(bytes);

            for (int i = 0; i < hash.Length; i++)
            {
                result += hash[i].ToString("x2");
            }

            return result;
        }

        #endregion

        #region TypeConvert

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

        public static decimal ToDecimal(this string value, decimal defaultValue = 0)
        {
            decimal result = 0;
            bool success = decimal.TryParse(value, out result);
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

        public static T ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion


        /// <summary>
        /// 替换掉第一个匹配的字符串
        /// </summary>
        public static string ReplaceFirst(this string value, string oldValue, string newValue)
        {
            string result = value;
            int index = value.IndexOf(oldValue);
            if (index > -1)
            {
                result = value.Remove(index, oldValue.Length).Insert(index, newValue);
            }
            return result;
        }

    }
}
