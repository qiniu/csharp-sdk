using System;
using System.IO;
using Newtonsoft.Json;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 断点续上传辅助函数Load/Save
    /// </summary>
    public class ResumeHelper
    {
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
            catch(Exception)
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
                resumeInfo.FileSize, resumeInfo.BlockIndex, resumeInfo.BlockCount, StringHelper.jsonJoin(resumeInfo.Contexts));

            using (FileStream fs = new FileStream(recordFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(jsonStr);
                }
            }
        }
    }
}
