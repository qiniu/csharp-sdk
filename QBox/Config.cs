using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox
{
    public class Config
    {
        public static string ACCESS_KEY = "<Please apply your access key>";
        public static string SECRET_KEY = "<Dont change here>";

        public static string REDIRECT_URI = "<RedirectURL>";
        public static string AUTHORIZATION_ENDPOINT = "<AuthURL>";
        public static string TOKEN_ENDPOINT = "https://acc.qbox.me/oauth2/token";

        public static string IO_HOST = "http://iovip.qbox.me";
        public static string FS_HOST = "https://fs.qbox.me";
        public static string RS_HOST = "http://rs.qbox.me:10100";
        public static string UP_HOST = "http://up.qbox.me";

        public static int BLOCK_SIZE = 1024 * 1024 * 4;
        public static int PUT_CHUNK_SIZE = 1024 * 256;
        public static int PUT_RETRY_TIMES = 3;
        public static int PUT_TIMEOUT = 300000; // 300s = 5m
    }
}
