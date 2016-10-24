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
            //Settings.LoadFromFile("test.cfg");
            Settings.Load();

            // 上传一个文件
            //SimpleUpload.uploadFile();

            // 上传后触发fop(数据处理)
            //UploadWithFop.uploadWithFop();

            // 生成已授权的下载链接
            //SimpleDownload.downloadFile();

            // 断点续上传
            //ResumableUpload.uploadBigFile();

            // 空间文件stat
            //BucketFileManagement.stat();

            // 删除空间文件
            //BucketFileManagement.delete();

            // 复制空间文件
            //BucketFileManagement.copy();

            // 移动空间文件
            //BucketFileManagement.move();

            // 修改空间文件的mimeType
            //BucketFileManagement.chgm();

            // 批量操作
            //BucketFileManagement.batch();

            // 取回文件并保存到空间
            //BucketFileManagement.fetch();

            // 镜像资源更新
            //BucketFileManagement.prefetch();

            // 列举所有bucket
            //BucketFileManagement.buckets();

            // 获取指定bucket的域名
            //BucketFileManagement.domains();

            // 获取空间文件列表
            // BucketFileManagement.listFiles();

            // 文件处理+保存处理结果
            //PfopWithPipeline.pfopAndSave();

            System.Console.ReadKey();
        }
    }
}