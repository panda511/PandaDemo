﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZCommon.Extention
{
    public static class ObjectExtension
    {
        public static string ToJson<T>(this T value, string DateTimeFormat) //yyyy-MM-dd HH:mm:ss
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = DateTimeFormat;
            return JsonConvert.SerializeObject(value, timeConverter);
        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T Clone<T>(this T t)
        {
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Object p = type.InvokeMember("", BindingFlags.CreateInstance, null, t, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(t, null);
                    pi.SetValue(p, value, null);
                }
            }
            return (T)p;
        }

        public static string ToString2(this object value)
        {
            string result = null;
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }
    }
}
