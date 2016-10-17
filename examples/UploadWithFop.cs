using Qiniu.Util;
using Qiniu.Storage;

namespace QiniuDemo
{
    public class UploadWithFop
    {
        public static void uploadAndFop()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "TARGET_BUCKET";
            string saveKey = "SAVE_KEY";
            string localFile = "LOCAL_FILE";

            //TODO
        }

    }
}