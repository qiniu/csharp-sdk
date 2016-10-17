using Qiniu.Util;
using Qiniu.Storage;

namespace QiniuDemo
{
    public class SimpleDownload
    {
        public static void downloadFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "TARGET_BUCKET";
            string saveKey = "SAVE_KEY";
            string localFile = "LOCAL_FILE";

            //TODO
			// PutPolicy.fops
        }

    }
}