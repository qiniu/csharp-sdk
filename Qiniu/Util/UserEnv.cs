using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qiniu.Util
{
    public class UserEnv
    {
        /// <summary>
        /// 获取home路径
        /// </summary>
        /// <returns></returns>
        public static string HomeFolder()
        {
            // Windows下Home目录 = %HOMEDRIVE% + %HOMEPATH%
            string homeFolder = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");

            if(string.IsNullOrEmpty(homeFolder))
            {
                // OSX/Ubuntu下Home目录 = $HOME
                homeFolder= Environment.GetEnvironmentVariable("HOME");
            }

            if (string.IsNullOrEmpty(homeFolder))
            {
                // 如果获取失败，就设置为./
                homeFolder = "./";
            }

            return homeFolder;
        }
    }
}
