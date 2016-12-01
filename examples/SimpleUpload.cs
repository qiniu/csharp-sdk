using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Http;

namespace CSharpSDKExamples
{
    public class SimpleUpload
    {
        public static void uploadFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string saveKey = "SAVE_KEY";
            string localFile = "LOCAL_FILE";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);

            UploadOptions uploadOptions = null;

            // 上传完毕事件处理
            UpCompletionHandler uploadCompleted = new UpCompletionHandler(OnUploadCompleted);

			// 方式1：使用UploadManager
            //默认设置 Qiniu.Common.Config.PUT_THRESHOLD = 512*1024;
            //可以适当修改,UploadManager会根据这个阈值自动选择是否使用分片(Resumable)上传	
            UploadManager um = new UploadManager();
            um.uploadFile(localFile, saveKey, token, uploadOptions, uploadCompleted);

			// 方式2：使用FormManager
            //FormUploader fm = new FormUploader();
            //fm.uploadFile(localFile, saveKey, token, uploadOptions, uploadCompleted);
        }

        private static void OnUploadCompleted(string key, ResponseInfo respInfo, string respJson)
        {
            // respInfo.StatusCode
            // respJson是返回的json消息，示例: { "key":"FILE","hash":"HASH","fsize":FILE_SIZE }
        }
    }
}