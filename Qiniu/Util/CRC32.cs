using System;
using System.IO;

namespace Qiniu.Util
{
    /// <summary>
    /// CRC32计算器
    /// </summary>
    public class CRC32
    {
        public const UInt32 IEEE = 0xedb88320;
        private UInt32[] Table;
        private UInt32 Value;

        public CRC32()
        {
            Value = 0;
            Table = MakeTable(IEEE);
        }

        public void Write(byte[] p, int offset, int count)
        {
            this.Value = Update(this.Value, this.Table, p, offset, count);
        }

        public UInt32 Sum32()
        {
            return this.Value;
        }

        private static UInt32[] MakeTable(UInt32 poly)
        {
            UInt32[] table = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 crc = (UInt32)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ poly;
                    else
                        crc >>= 1;
                }
                table[i] = crc;
            }
            return table;
        }

        public static UInt32 Update(UInt32 crc, UInt32[] table, byte[] p, int offset, int count)
        {
            crc = ~crc;
            for (int i = 0; i < count; i++)
            {
                crc = table[((byte)crc) ^ p[offset + i]] ^ (crc >> 8);
            }
            return ~crc;
        }

        /// <summary>
        /// 计算字节数据的crc32值
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <param name="length">长度</param>
        /// <returns>crc32值</returns>
        public static UInt32 CheckSumBytes(byte[] data)
        {
            CRC32 crc = new CRC32();
            crc.Write(data, 0, data.Length);
            return crc.Sum32();
        }

        public static UInt32 CheckSumSlice(byte[] data, int offset, int count)
        {
            CRC32 crc = new CRC32();
            crc.Write(data, offset, count);
            return crc.Sum32();
        }

        /// <summary>
        /// 计算沙盒文件的crc32值
        /// </summary>
        /// <param name="filePath">沙盒文件全路径</param>
        /// <returns>crc32值</returns>
        public static UInt32 CheckSumFile(string filePath)
        {
            CRC32 crc = new CRC32();
            int bufferLen = 32 * 1024;
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[bufferLen];
                while (true)
                {
                    int n = fs.Read(buffer, 0, bufferLen);
                    if (n == 0)
                        break;
                    crc.Write(buffer, 0, n);
                }
            }
            return crc.Sum32();
        }
    }
}