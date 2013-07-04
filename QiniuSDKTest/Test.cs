using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Conf;

namespace QiniuSDKTest
{
    public class Test
    {
        private void Init()
        {
            Config.ACCESS_KEY = "gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
            Config.SECRET_KEY = "OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";
        }

        public Test()
        {
            Init();
        }
        public void Printf(string str)
        {
            Console.WriteLine(str);
        }
    }
}
