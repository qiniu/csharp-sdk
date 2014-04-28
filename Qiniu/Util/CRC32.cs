using System;
using System.IO;

namespace Qiniu.Util
{
	public class CRC32
	{
		public const UInt32 IEEE = 0xedb88320;
		private UInt32[] Table;
		private UInt32 Value;

		public CRC32 ()
		{
			Value = 0;
			Table = MakeTable (IEEE);
		}

		public void Write (byte[] p, int offset, int count)
		{
			this.Value = Update (this.Value, this.Table, p, offset, count);
		}

		public UInt32 Sum32 ()
		{
			return this.Value;
		}

		private static UInt32[] MakeTable (UInt32 poly)
		{
			UInt32[] table = new UInt32[256];
			for (int i = 0; i < 256; i++) {
				UInt32 crc = (UInt32)i;
				for (int j = 0; j < 8; j++) {
					if ((crc & 1) == 1)
						crc = (crc >> 1) ^ poly;
					else
						crc >>= 1;
				}
				table [i] = crc;
			}
			return table;
		}

		public static UInt32 Update (UInt32 crc, UInt32[] table, byte[] p, int offset, int count)
		{
			crc = ~crc;
			for (int i = 0; i < count; i++) {
				crc = table [((byte)crc) ^ p [offset + i]] ^ (crc >> 8);
			}
			return ~crc;
		}

		public static UInt32 CheckSumBytes (byte[] data,int length)
		{
			CRC32 crc = new CRC32 ();
			crc.Write (data, 0, length);
			return crc.Sum32 ();
		}

		public static UInt32 CheckSumFile (string fileName)
		{
			CRC32 crc = new CRC32 ();
			int bufferLen = 32 * 1024;
			using (FileStream fs = File.OpenRead(fileName)) {
				byte[] buffer = new byte[bufferLen];
				while (true) {
					int n = fs.Read (buffer, 0, bufferLen);
					if (n == 0)
						break;
					crc.Write (buffer, 0, n);
				}
			}
			return crc.Sum32 ();
		}
	}
}
