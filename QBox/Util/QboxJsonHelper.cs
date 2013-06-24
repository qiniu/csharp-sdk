using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QBox.Util
{
     public class QboxJsonHelper
    {
         public static string JsonEncode(object obj)
         {
             JsonSerializerSettings setting = new JsonSerializerSettings();
             setting.NullValueHandling = NullValueHandling.Ignore;
             string flag = JsonConvert.SerializeObject(obj, setting);
             return flag;

         }
    }
}
