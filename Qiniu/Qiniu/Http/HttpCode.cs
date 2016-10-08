using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Created by fengyh
// 2016-08-17

namespace Qiniu.Http
{
    #region HTTP-CODE
    // 参考http://developer.qiniu.com/article/developer/response-body.html
    // @fengyh 2016-08-17 18:28
    public enum HCODE
    {
        OK = 200,
        PART_OK = 298,
        REQUEST_ERR = 400,
        AUTHENTICATION_ERR = 401,
        ACCESS_DENIED = 403,
        OBJECT_NOT_FOUND = 404,
        BAD_REQUEST = 405,
        CRC32_CHECk_ERR = 406,
        FSIZE_LIMIT_EXCEED = 413,
        ACCOUNT_BLOCKED = 419,
        MIRR2ORG_ERR = 478,
        BAD_GATEWAY = 502,
        SERVICE_UNAVAILABLE = 503,
        SERVER_TIMEOUT = 504,
        TOO_FREQUENT = 573,
        CALLBACK_FAILED = 579,
        SERVER_SIDE_FAILURE = 599,
        CONTENT_MODIFIED = 608,
        RESOURCE_NOT_EXSISTS = 612,
        RESOURCE_ALREADY_EXISTS = 614,
        NUM_OF_BUCKETS_EXCEEDS = 630,
        BUCKET_NOT_EXSISTS = 631,
        INVALID_LIST_MARKER = 640,
        RESUMABLE_UPLOAD_ERR = 701,
    };

    #endregion HTTP-CODE

    /// <summary>
    /// 用于解析HTTP Status Code
    /// 参考http://developer.qiniu.com/article/developer/response-body.html
    /// @fengyh 2016-08-17 18:28
    /// </summary>
    public class HttpCode
    {
        private static Dictionary<HCODE, string> codeTable  =  new Dictionary<HCODE, string>() 
        {
            #region TABLE-ITEMS
            {HCODE.OK,"success"},
            {HCODE.PART_OK, "partial success"},
            {HCODE.REQUEST_ERR, "request error or null-response"},
            {HCODE.AUTHENTICATION_ERR, "authentication failed"},
            {HCODE.ACCESS_DENIED, "access denied"},
            {HCODE.OBJECT_NOT_FOUND, "object not found"},
            {HCODE.BAD_REQUEST, "bad request"},
            {HCODE.CRC32_CHECk_ERR, "crc32 check failed"},
            {HCODE.FSIZE_LIMIT_EXCEED, "size limit exceeded"},
            {HCODE.ACCOUNT_BLOCKED, "account blocked"},
            {HCODE.MIRR2ORG_ERR, "mirror-to-origin failed"},
            {HCODE.BAD_GATEWAY, "bad gateway"},
            {HCODE.SERVICE_UNAVAILABLE, "service unavailable"},
            {HCODE.SERVER_TIMEOUT, "server timeout"},
            {HCODE.TOO_FREQUENT, "too frequently access to one file"},
            {HCODE.CALLBACK_FAILED, "upload success but callbak failed"},
            {HCODE.SERVER_SIDE_FAILURE, "failure on server-side-ops"},
            {HCODE.CONTENT_MODIFIED, "content modified"},
            {HCODE.RESOURCE_NOT_EXSISTS, "resource not found or removed"},
            {HCODE.RESOURCE_ALREADY_EXISTS, "resource already exists"},
            {HCODE.NUM_OF_BUCKETS_EXCEEDS, "the number of space is up to limit"},
            {HCODE.BUCKET_NOT_EXSISTS, "bucket not exsists"},
            {HCODE.INVALID_LIST_MARKER, "invalid param marker in list-interface"},
            {HCODE.RESUMABLE_UPLOAD_ERR, "resumable upload error"}
            #endregion TABLE-ITEMS
        };

        public static string GetMessage(int code)
        {
            string msg = string.Format("HTTP_CODE({0})", code);

            if(codeTable.ContainsKey((HCODE)code))
            {
                msg = codeTable[(HCODE)code];
            }

            return msg;
        }

        public static bool GetErrorMessage(int code,out string errorMsg)
        {
            bool isOtherCode = false;

            switch (code)
            {
                case (int)HCODE.PART_OK:
                case (int)HCODE.REQUEST_ERR:
                case (int)HCODE.AUTHENTICATION_ERR:
                case (int)HCODE.ACCESS_DENIED:
                case (int)HCODE.OBJECT_NOT_FOUND:
                case (int)HCODE.BAD_REQUEST:
                case (int)HCODE.CRC32_CHECk_ERR:
                case (int)HCODE.FSIZE_LIMIT_EXCEED:
                case (int)HCODE.ACCOUNT_BLOCKED:
                case (int)HCODE.MIRR2ORG_ERR:
                case (int)HCODE.BAD_GATEWAY:
                case (int)HCODE.SERVICE_UNAVAILABLE:
                case (int)HCODE.SERVER_TIMEOUT:
                case (int)HCODE.TOO_FREQUENT:
                case (int)HCODE.CALLBACK_FAILED:
                case (int)HCODE.SERVER_SIDE_FAILURE:
                case (int)HCODE.CONTENT_MODIFIED:
                case (int)HCODE.RESOURCE_NOT_EXSISTS:
                case (int)HCODE.RESOURCE_ALREADY_EXISTS:
                case (int)HCODE.NUM_OF_BUCKETS_EXCEEDS:
                case (int)HCODE.BUCKET_NOT_EXSISTS:
                case (int)HCODE.INVALID_LIST_MARKER:
                case (int)HCODE.RESUMABLE_UPLOAD_ERR:
                    errorMsg = HttpCode.GetMessage(code);
                    break;
                default:
                    errorMsg = "";
                    isOtherCode = true;
                    break;
            }

            return isOtherCode;
        }
    }
}
