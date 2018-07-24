using System.IO;

namespace Qiniu.Util
{
    /// <summary>
    ///     CRC32计算器
    /// </summary>
    public class CRC32
    {
        /// <summary>
        ///     magic
        /// </summary>
        public const uint IEEE = 0xedb88320;

        private readonly uint[] Table;
        private uint Value;

        /// <summary>
        ///     初始化
        /// </summary>
        public CRC32()
        {
            Value = 0;
            Table = makeTable(IEEE);
        }

        /// <summary>
        ///     写入
        /// </summary>
        /// <param name="p">字节数据</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="count">字节数</param>
        public void Write(byte[] p, int offset, int count)
        {
            Value = Update(Value, Table, p, offset, count);
        }

        /// <summary>
        ///     校验和
        /// </summary>
        /// <returns>校验和</returns>
        public uint Sum()
        {
            return Value;
        }

        private static uint[] makeTable(uint poly)
        {
            var table = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var crc = (uint)i;
                for (var j = 0; j < 8; j++)
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ poly;
                    }
                    else
                    {
                        crc >>= 1;
                    }

                table[i] = crc;
            }

            return table;
        }

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="crc">crc32</param>
        /// <param name="table">表</param>
        /// <param name="p">字节数据</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="count">字节数</param>
        /// <returns></returns>
        public static uint Update(uint crc, uint[] table, byte[] p, int offset, int count)
        {
            crc = ~crc;
            for (var i = 0; i < count; i++) crc = table[(byte)crc ^ p[offset + i]] ^ (crc >> 8);
            return ~crc;
        }

        /// <summary>
        ///     计算字节数据的crc32值
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>crc32值</returns>
        public static uint CheckSumBytes(byte[] data)
        {
            var crc = new CRC32();
            crc.Write(data, 0, data.Length);
            return crc.Sum();
        }

        /// <summary>
        ///     检验
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <param name="offset">偏移位置</param>
        /// <param name="count">字节数</param>
        /// <returns></returns>
        public static uint CheckSumSlice(byte[] data, int offset, int count)
        {
            var crc = new CRC32();
            crc.Write(data, offset, count);
            return crc.Sum();
        }

        /// <summary>
        ///     计算沙盒文件的crc32值
        /// </summary>
        /// <param name="filePath">沙盒文件全路径</param>
        /// <returns>crc32值</returns>
        public static uint checkSumFile(string filePath)
        {
            var crc = new CRC32();
            var bufferLen = 32 * 1024;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[bufferLen];
                while (true)
                {
                    var n = fs.Read(buffer, 0, bufferLen);
                    if (n == 0)
                    {
                        break;
                    }

                    crc.Write(buffer, 0, n);
                }
            }

            return crc.Sum();
        }
    }
}
