using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Extention
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
    }
}
