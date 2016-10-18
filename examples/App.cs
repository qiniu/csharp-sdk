namespace QiniuDemo
{
    /// <summary>
    /// 示例代码中的bucket,saveKey,localFile等请自行设置
    /// </summary>
    class ConsoleDemo
    {
        static void Main(string[] args)
        {
			// 载入密钥设置(AK&SK)		
            Settings.LoadFromFile();

            // 上传一个文件
            //SimpleUpload.uploadFile();

            // 上传后触发fop(数据处理)
            //UploadWithFop.uploadWithFop();

            // 生成已授权的下载链接
            //SimpleDownload.downloadFile();

            // 断点续上传
            //ResumableUpload.uploadBigFile();
			
            // 取回文件并保存到空间
            //BucketFileManagemt.fetch();

            // 空间文件复制
            //BucketFileManagemt.copy();

            // 批量操作
            BucketFileManagemt.batch();

            System.Console.ReadKey();
        }
    }
}