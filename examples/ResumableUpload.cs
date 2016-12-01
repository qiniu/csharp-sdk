using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Storage.Persistent;
using Qiniu.Http;

namespace CSharpSDKExamples
{
    public class ResumableUpload
    {
        public static void uploadBigFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
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

            UploadOptions uploadOptions = new UploadOptions(
                null, // ExtraParams
                null, // MimeType
                false,  // CheckCrc32
                new UpProgressHandler(OnUploadProgressChanged), // 上传进度
                null // CancelSignal
                );

            UpCompletionHandler uploadCompleted = new UpCompletionHandler(OnUploadCompleted); // 上传完毕

            ResumeUploader ru = new ResumeUploader(
                rr,               // 续传记录
                recordFile,       // 续传记录文件
                localFile,        // 待上传的本地文件
                saveKey,          // 要保存的文件名
                token,            // 上传凭证
                uploadOptions,    // 上传选项(其中包含进度处理)，可为null
                uploadCompleted   // 上传完毕事件处理
                ); 

            ru.uploadFile();
        }

        private static void OnUploadProgressChanged(string key,double percent)
        {
            // percent = [0(开始)~1.0(完成)]
        }

        private static void OnUploadCompleted(string key,ResponseInfo respInfo,string respJson)
        {
            // respInfo.StatusCode
            // respJson是返回的json消息，示例: { "key":"FILE","hash":"HASH","fsize":FILE_SIZE }
        }
    }
}