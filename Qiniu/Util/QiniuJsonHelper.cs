using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Qiniu.Util
{
     public static class QiniuJsonHelper
    {
         public static string JsonEncode(object obj)
         {
             JsonSerializerSettings setting = new JsonSerializerSettings();
             setting.NullValueHandling = NullValueHandling.Ignore;
             return JsonConvert.SerializeObject(obj, setting);
         }
         //public static object JsonDecode<T>(string text)
         //{
         //    JsonConvert.DeserializeObject<T>(text);
         //}
         public static T ToObject<T>(this string value)
         {
             return JsonConvert.DeserializeObject<T>(value);
         }
    }
}
