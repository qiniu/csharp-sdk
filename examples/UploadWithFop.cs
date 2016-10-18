using Qiniu.Util;
using Qiniu.Storage;

namespace QiniuDemo
{
    public class UploadWithFop
    {
        public static void uploadWithFop()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "TARGET_BUCKET";
            string saveKey = "SAVE_KEY";
            string localFile = "LOCAL_FILE";

            // 如果想要将处理结果保存到SAVEAS_BUCKET空间下，文件名为SAVEAS_KEY
            // 可以使用savas参数 <FOPS>|saveas/<encodedUri>
            // encodedUri = StringUtils.urlSafeBase64Encode("SAVEAS_BUCKET" + ":" + "SAVEAS_KEY");
            string fops = "FOPS";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.PersistentOps = fops;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);

            FormUploader fm = new FormUploader();
            fm.uploadFile(localFile, saveKey, token, null, null);
        }
    }
}
