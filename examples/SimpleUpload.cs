using Qiniu.Util;
using Qiniu.Storage;

namespace QiniuDemo
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

			// 方式1：使用UploadManager
            UploadManager um = new UploadManager();
            um.uploadFile(localFile, saveKey, token, null, null);

			// 方式2：使用FormManager
            //FormUploader fm = new FormUploader();
            //fm.uploadFile(localFile, saveKey, token, null, null);
        }
    }
}