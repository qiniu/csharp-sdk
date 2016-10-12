using System;
using System.IO;

namespace Qiniu.Storage.Persistent
{
    /// <summary>
    /// 分片上传进度记录器
    /// </summary>
    public class ResumeRecorder
    {
        //上传进度记录目录
        private string dir;

        /// <summary>
        /// 构建上传进度记录器
        /// </summary>
        /// <param name="dir">保存目录</param>
        public ResumeRecorder(string dir)
        {
            this.dir = dir;
            createDirIfNotExist();
        }

        /// <summary>
        /// 如果指定目录不存在，则创建
        /// </summary>
        private void createDirIfNotExist()
        {
			if(!Directory.Exists (this.dir))
            {
				Directory.CreateDirectory (this.dir);
            }
        }

        /// <summary>
        /// 写入或更新上传进度记录
        /// </summary>
        /// <param name="key">记录文件名</param>
        /// <param name="data">上传进度数据</param>
        public void set(string key, byte[] data)
        {
            string filePath = Path.Combine(this.dir, key);
            using (FileStream stream =
                new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }

        /// <summary>
        /// 获取上传进度记录
        /// </summary>
        /// <param name="key">记录文件名</param>
        /// <returns>上传进度数据</returns>
        public byte[] get(string key)
        {
            byte[] data = null;
            string filePath = Path.Combine(this.dir, key);
            try
            {
				using (FileStream stream =
					new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                }
            }
            catch (Exception)
            {

            }
            return data;
        }

        /// <summary>
        /// 删除上传进度记录
        /// </summary>
        /// <param name="key">记录文件名</param>
        public void del(string key)
        {
            string filePath = Path.Combine(this.dir, key);
            try
            {
				File.Delete(filePath);
            }
            catch (Exception)
            {

            }
        }
    }
}
