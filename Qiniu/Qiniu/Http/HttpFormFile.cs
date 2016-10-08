using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Qiniu.Http
{
    public class HttpFormFile
    {
        public string Filename { set; get; }
        public string ContentType { set; get; }
        public HttpFileType BodyType { set; get; }
        public Stream BodyStream { set; get; }
        public string BodyFile { set; get; }
        public byte[] BodyBytes { set; get; }
        public int Offset { set; get; }
        public int Count { set; get; }

        private HttpFormFile()
        {
        }

        private static HttpFormFile newObject(string filename, string contentType, object body)
        {
            HttpFormFile obj = new HttpFormFile();
            obj.Filename = filename;
            obj.ContentType = contentType;
            if (body is Stream)
            {
                obj.BodyStream = (Stream)body;
            }
            else if (body is byte[])
            {
                obj.BodyBytes = (byte[])body;
            }
            else if (body is string)
            {
                obj.BodyFile = (string)body;
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contentType"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static HttpFormFile NewFileFromPath(string filename, string contentType, string filepath)
        {
            HttpFormFile obj = newObject(filename, contentType, filepath);
            obj.BodyType = HttpFileType.FILE_PATH;
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contentType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static HttpFormFile NewFileFromStream(string filename, string contentType, Stream stream)
        {
            HttpFormFile obj = newObject(filename, contentType, stream);
            obj.BodyType = HttpFileType.FILE_STREAM;
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contentType"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static HttpFormFile NewFileFromBytes(string filename, string contentType, byte[] fileData)
        {
            HttpFormFile obj = newObject(filename, contentType, fileData);
            obj.BodyType = HttpFileType.DATA_BYTES;
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contentType"></param>
        /// <param name="fileData"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static HttpFormFile NewFileFromSlice(string filename, string contentType, byte[] fileData, int offset, int count)
        {
            HttpFormFile obj = newObject(filename, contentType, fileData);
            obj.BodyType = HttpFileType.DATA_SLICE;
            obj.Offset = offset;
            obj.Count = count;
            return obj;
        }
    }
}
