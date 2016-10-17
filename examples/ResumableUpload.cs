using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Storage.Persistent;
using Qiniu.Http;

namespace QiniuDemo
{
    public class ResumableUpload
    {
        public static void uploadBigFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "TARGET_BUCKET";
            string saveKey = "SAVE_KEY";
            string localFile = "LOCAL_FILE";
            string recordPath = "RECORD_PATH";
            string recordFile = "RECORD_FILE";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;

            string token = Auth.createUploadToken(putPolicy, mac);

            ResumeRecorder rr = new ResumeRecorder(recordPath);
            ResumeUploader ru = new ResumeUploader(rr, recordFile, localFile, saveKey, token, null, null);
            
            ru.uploadFile();
        }

    }
}