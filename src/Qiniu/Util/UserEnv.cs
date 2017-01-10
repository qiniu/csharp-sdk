using System;

namespace Qiniu.Util
{
    /// <summary>
    /// 环境变量-用户路径
    /// </summary>
    public class UserEnv
    {
        /// <summary>
        /// 获取home路径
        /// </summary>
        /// <returns>HOME路径</returns>
        public static string getHomeFolder()
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
