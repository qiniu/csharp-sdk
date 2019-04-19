using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.RTC
{
    /// <summary>
    /// 实时音视频服务端
    /// </summary>
    public class RTCManager
    {
        private const string RTC_API_HOST = "http://rtc.qiniuapi.com";
        private Auth _auth;

        public RTCManager(Mac mac)
        {
            _auth = new Auth(mac);
        }

        /// <summary>
        /// RoomToken签发服务
        /// </summary>
        /// <param name="appId">房间所属账号的AppID</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="userId">请求加入房间的用户ID</param>
        /// <param name="expireAt">鉴权的有效时间</param>
        /// <param name="permission">该用户的房间管理权限</param>
        /// <returns>RoomToken</returns>
        public string GetRoomToken(string appId, string roomName, string userId, long expireAt, string permission)
        {
            RoomTokenRequest request = new RoomTokenRequest(appId, roomName, userId, expireAt, permission);
            return _auth.CreateUploadToken(request.ToJsonStr());
        }
    }
}