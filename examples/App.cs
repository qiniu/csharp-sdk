namespace QiniuDemo
{
    class ConsoleDemo
    {
        static void Main(string[] args)
        {
			// Load settings(AK&SK)		
            Settings.LoadFromFile();

            //SimpleUpload.uploadFile();

            //SimpleDownload.downloadFile();

            //ResumableUpload.uploadBigFile();
			
            //BucketFileManagemt.fetch();

            //BucketFileMgmt.copy();

            //BucketFileMgmt.batch();

            //UploadWithFop.uploadAndFop();

            System.Console.ReadKey();
        }

    }
}