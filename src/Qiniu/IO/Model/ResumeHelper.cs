using System;
using System.IO;
using Newtonsoft.Json;
using Qiniu.Util;

#if Net45||Net46||NetCore
using System.Threading.Tasks;
#endif


#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
#endif

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 断点续上传辅助函数Load/Save
    /// </summary>
    public class ResumeHelper
    {

        /// <summary>
        /// 生成默认的断点记录文件名称
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标key</param>
        /// <returns>用于记录断点信息的文件名</returns>
        public static string GetDefaultRecordKey(string localFile, string saveKey)
        {
            return "QiniuRU_" + Hashing.CalcMD5X(localFile + saveKey);
        }

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        /// <summary>
        /// 尝试从从文件载入断点信息
        /// </summary>
        /// <param name="recordFile">断点记录文件</param>
        /// <returns>断点信息</returns>
        public static ResumeInfo Load(string recordFile)
        {
            ResumeInfo resumeInfo = null;

            try
            {
                using (FileStream fs = new FileStream(recordFile, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string jsonStr = sr.ReadToEnd();
                        resumeInfo = JsonConvert.DeserializeObject<ResumeInfo>(jsonStr);
                    }
                }
            }
            catch (Exception)
            {
                resumeInfo = null;
            }

            return resumeInfo;
        }

        /// <summary>
        /// 保存断点信息到文件
        /// </summary>
        /// <param name="resumeInfo">断点信息</param>
        /// <param name="recordFile">断点记录文件</param>
        public static void Save(ResumeInfo resumeInfo, string recordFile)
        {
            string jsonStr = string.Format("{{\"fileSize\":{0}, \"blockIndex\":{1}, \"blockCount\":{2}, \"contexts\":[{3}]}}",
                resumeInfo.FileSize, resumeInfo.BlockIndex, resumeInfo.BlockCount, StringHelper.JsonJoin(resumeInfo.Contexts));

            using (FileStream fs = new FileStream(recordFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(jsonStr);
                }
            }
        }

#endif

#if Net45 || Net46 || NetCore

        /// <summary>
        /// 尝试从从文件载入断点信息
        /// </summary>
        /// <param name="recordFile">断点记录文件</param>
        /// <returns>断点信息</returns>
        public static async Task<ResumeInfo> LoadAsync(string recordFile)
        {
            ResumeInfo resumeInfo = null;

            try
            {
                using (FileStream fs = new FileStream(recordFile, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string jsonStr = await sr.ReadToEndAsync();
                        resumeInfo = JsonConvert.DeserializeObject<ResumeInfo>(jsonStr);
                    }
                }
            }
            catch (Exception)
            {
                resumeInfo = null;
            }

            return resumeInfo;
        }

        /// <summary>
        /// 保存断点信息到文件
        /// </summary>
        /// <param name="resumeInfo">断点信息</param>
        /// <param name="recordFile">断点记录文件</param>
        public static async Task SaveAsync(ResumeInfo resumeInfo, string recordFile)
        {
            string jsonStr = string.Format("{{\"fileSize\":{0}, \"blockIndex\":{1}, \"blockCount\":{2}, \"contexts\":[{3}]}}",
                resumeInfo.FileSize, resumeInfo.BlockIndex, resumeInfo.BlockCount, StringHelper.JsonJoin(resumeInfo.Contexts));

            using (FileStream fs = new FileStream(recordFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    await sw.WriteAsync(jsonStr);
                }
            }
        }

#endif

#if WINDOWS_UWP

        /// <summary>
        /// 尝试从从文件载入断点信息
        /// </summary>
        /// <param name="recordFile">断点记录文件</param>
        /// <returns>断点信息</returns>
        public static async Task<ResumeInfo> LoadAsync(StorageFile recordFile)
        {
            ResumeInfo resumeInfo = null;

            try
            {
                JsonConvert.DeserializeObject<ResumeInfo>(await FileIO.ReadTextAsync(recordFile));
            }
            catch (Exception)
            {
                resumeInfo = null;
            }

            return resumeInfo;
        }

        /// <summary>
        /// 保存断点信息到文件
        /// </summary>
        /// <param name="resumeInfo">断点信息</param>
        /// <param name="recordFile">断点记录文件</param>
        public static async Task SaveAsync(ResumeInfo resumeInfo, StorageFile recordFile)
        {
            string jsonStr = string.Format("{{\"fileSize\":{0}, \"blockIndex\":{1}, \"blockCount\":{2}, \"contexts\":[{3}]}}",
                resumeInfo.FileSize, resumeInfo.BlockIndex, resumeInfo.BlockCount, StringHelper.JsonJoin(resumeInfo.Contexts));

            await FileIO.WriteTextAsync(recordFile, jsonStr);
        }

#endif

    }
}
