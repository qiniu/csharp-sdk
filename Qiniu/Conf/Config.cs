﻿using System;
using System.Text;

namespace Qiniu.Conf
{
    public class Config
    {
        public static string VERSION = "6.1.8";

        public static string USER_AGENT = getUa();
        #region 帐户信息
        /// <summary>
        /// 七牛提供的公钥，用于识别用户
        /// </summary>
        public static string ACCESS_KEY = "<Please apply your access key>";
        /// <summary>
        /// 七牛提供的秘钥，不要在客户端初始化该变量
        /// </summary>
        public static string SECRET_KEY = "<Dont send your secret key to anyone>";
        #endregion
        #region 七牛服务器地址
        /// <summary>
        /// 七牛资源管理服务器地址
        /// </summary>
        public static string RS_HOST = "http://rs.Qbox.me";
        /// <summary>
        /// 七牛资源上传服务器地址.
        /// </summary>
        public static string UP_HOST = "http://up.qiniu.com";
        /// <summary>
        /// 七牛资源列表服务器地址.
        /// </summary>
        public static string RSF_HOST = "http://rsf.Qbox.me";

        public static string PREFETCH_HOST = "http://iovip.qbox.me";

        public static string API_HOST = "http://api.qiniu.com";

        /// <summary>
        /// Http Reqeust timeout, default is 600000(10 mins).
        /// </summary>
        public static int HTTP_REQUEST_TIMEOUT = 600000;

        #endregion
        /// <summary>
        /// 七牛SDK对所有的字节编码采用utf-8形式 .
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 初始化七牛帐户、请求地址等信息，不应在客户端调用。
        /// </summary>
         public static void Init()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["USER_AGENT"] != null) 
            {
                USER_AGENT = System.Configuration.ConfigurationManager.AppSettings["USER_AGENT"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["ACCESS_KEY"] != null)
            {
                ACCESS_KEY = System.Configuration.ConfigurationManager.AppSettings["ACCESS_KEY"];   
            }
            if (System.Configuration.ConfigurationManager.AppSettings["SECRET_KEY"] != null)
            {
                SECRET_KEY = System.Configuration.ConfigurationManager.AppSettings["SECRET_KEY"];  
            }
            if (System.Configuration.ConfigurationManager.AppSettings["RS_HOST"] != null)
            {
                RS_HOST = System.Configuration.ConfigurationManager.AppSettings["RS_HOST"];  
            }
            if (System.Configuration.ConfigurationManager.AppSettings["UP_HOST"] != null)
            {
                UP_HOST = System.Configuration.ConfigurationManager.AppSettings["UP_HOST"];   
            }
            if (System.Configuration.ConfigurationManager.AppSettings["RSF_HOST"] != null)
            {
                RSF_HOST = System.Configuration.ConfigurationManager.AppSettings["RSF_HOST"];  
            }
            if (System.Configuration.ConfigurationManager.AppSettings["PREFETCH_HOST"] != null)
            {
                PREFETCH_HOST = System.Configuration.ConfigurationManager.AppSettings["PREFETCH_HOST"];
            }
            if (System.Configuration.ConfigurationManager.AppSettings["HTTP_REQUEST_TIMEOUT"] != null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["HTTP_REQUEST_TIMEOUT"], out HTTP_REQUEST_TIMEOUT);
            }
        }
        private static string getUa()
        {
            return "QiniuCsharp/"+ VERSION + " (" + Environment.OSVersion.Version.ToString() + "; )";
        }
    }
}
