using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.CDN;
using Qiniu.CDN.Model;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace QiniuExampleUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string AK = "<ACCESS_KEY>";
        private string SK = "<SECRET_KEY>";

        private StorageFile localFile;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            // 设置要支持的文件类型或者任意类型
            FileOpenPicker opx = new FileOpenPicker();
            opx.FileTypeFilter.Add(".mp4");
            opx.FileTypeFilter.Add(".txt");
            opx.FileTypeFilter.Add(".png");
            localFile = await opx.PickSingleFileAsync();
            TextBox_LocalFile.Text = localFile.Path;
        }

        private async void Button_UploadFile_Click(object sender, RoutedEventArgs e)
        {
            string saveKey = "uwp-upload-test-1.mp4";

            Mac mac = new Mac(AK, SK);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = "test";
            putPolicy.SetExpires(30);

            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);

            //UploadProgressHandler upph = new UploadProgressHandler(MyUploadProgresHandler);

            //UploadManager um = new UploadManager();
            //um.SetUploadProgressHandler(upph);
            //var result = await um.UploadFileAsync(localFile, saveKey, token);

            FormUploader fu = new FormUploader(true);

            var result =  await fu.UploadFileAsync(localFile, saveKey, token);

            TextBox_Info.Text = result.ToString();
        }

        private void MyUploadProgresHandler(long u,long t)
        {
            ProgressBar_UploadProgress.Value = 1.0 * u / t;
            TextBox_Info.Text = string.Format("上传进度: {0:P}", 1.0 * u / t);
        }

        private async void Button_UploadData_Click(object sender, RoutedEventArgs e)
        {
            string saveKey = "uwp-upload-data-test-1";

            Mac mac = new Mac(AK, SK);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = "test";
            putPolicy.SetExpires(30);

            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);

            UploadProgressHandler upph = new UploadProgressHandler(MyUploadProgresHandler);

            UploadManager um = new UploadManager();
            um.SetUploadProgressHandler(upph);

            byte[] data = await FormUploader.ReadToByteArrayAsync(localFile);
            var result = await um.UploadDataAsync(data, saveKey, token);
            TextBox_Info.Text = result.ToString();
        }
    }
}
