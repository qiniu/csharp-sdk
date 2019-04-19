using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.RTC
{
    /// <summary>
    /// 房间管理凭证-请求
    /// </summary>
    public class RoomTokenRequest
    {
        /// <summary>
        /// 房间所属账号的AppID
        /// </summary>
        [JsonProperty("appId")]
        public string AppID { get; set; }
        /// <summary>
        /// 房间名称
        /// </summary>
        [JsonProperty("roomName")]
        public string RoomName { get; set; }
        /// <summary>
        /// 请求加入房间的用户ID
        /// </summary>
        [JsonProperty("userId")]
        public string UserID { get; set; }
        /// <summary>
        /// 鉴权的有效时间，传入以秒为单位的 64 位 Unix 绝对时间，token 将在该时间后失效
        /// </summary>
        [JsonProperty("expireAt")]
        public long ExpireAt { get; set; }
        /// <summary>
        /// 该用户的房间管理权限，"admin" 或 "user"
        /// </summary>
        [JsonProperty("permission")]
        public string Permission { get; set; }

        /// <summary>
        /// 初始化(所有成员为空，需要后续赋值)
        /// </summary>
        public RoomTokenRequest()
        {
            AppID = "";
            RoomName = "";
            UserID = "";
            ExpireAt = 0;
            Permission = "";
        }

        /// <summary>
        /// 初始化所有成员
        /// </summary>
        /// <param name="appId">房间所属账号的AppID</param>
        /// <param name="roomName">房间名称</param>
        /// <param name="userId">请求加入房间的用户ID</param>
        /// <param name="expireAt">鉴权的有效时间</param>
        /// <param name="permission">该用户的房间管理权限</param>
        public RoomTokenRequest(string appId, string roomName, string userId, long expireAt, string permission)
        {
            AppID = appId;
            RoomName = roomName;
            UserID = userId;
            ExpireAt = expireAt;
            Permission = permission;
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <returns>请求内容的JSON字符串</returns>
        public string ToJsonStr()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}