namespace Qiniu.Util
{
    /// <summary>
    ///     MD5算法的3rdParty实现
    ///     参考https://github.com/Dozer74/MD5
    /// </summary>
    public class LabMd5
    {
        #region Table

        /// <summary>
        ///     table 4294967296*sin(i)
        /// </summary>
        private static readonly uint[] T =
        {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x2441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x4881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        #endregion Table

        private readonly uint[] _x = new uint[16];

        /// <summary>
        ///     ComputeHash
        /// </summary>
        public string ComputeHash(byte[] bytes)
        {
            var dg = new Digest();
            var bMsg = CreatePaddedBuffer(bytes);

            var mesLength = (uint)(bMsg.Length * 8) / 32;

            for (uint i = 0; i < mesLength / 16; i++)
            {
                CopyBlock(bMsg, i);
                Transform(ref dg.A, ref dg.B, ref dg.C, ref dg.D);
            }

            return dg.ToString();
        }

        private void Transform(ref uint a, ref uint b, ref uint c, ref uint d)
        {
            var aa = a;
            var bb = b;
            var cc = c;
            var dd = d;

            /* Round 1 */
            F(ref a, b, c, d, 0, 7, 1);
            F(ref d, a, b, c, 1, 12, 2);
            F(ref c, d, a, b, 2, 17, 3);
            F(ref b, c, d, a, 3, 22, 4);
            F(ref a, b, c, d, 4, 7, 5);
            F(ref d, a, b, c, 5, 12, 6);
            F(ref c, d, a, b, 6, 17, 7);
            F(ref b, c, d, a, 7, 22, 8);
            F(ref a, b, c, d, 8, 7, 9);
            F(ref d, a, b, c, 9, 12, 10);
            F(ref c, d, a, b, 10, 17, 11);
            F(ref b, c, d, a, 11, 22, 12);
            F(ref a, b, c, d, 12, 7, 13);
            F(ref d, a, b, c, 13, 12, 14);
            F(ref c, d, a, b, 14, 17, 15);
            F(ref b, c, d, a, 15, 22, 16);

            /* Round 2 */
            G(ref a, b, c, d, 1, 5, 17);
            G(ref d, a, b, c, 6, 9, 18);
            G(ref c, d, a, b, 11, 14, 19);
            G(ref b, c, d, a, 0, 20, 20);
            G(ref a, b, c, d, 5, 5, 21);
            G(ref d, a, b, c, 10, 9, 22);
            G(ref c, d, a, b, 15, 14, 23);
            G(ref b, c, d, a, 4, 20, 24);
            G(ref a, b, c, d, 9, 5, 25);
            G(ref d, a, b, c, 14, 9, 26);
            G(ref c, d, a, b, 3, 14, 27);
            G(ref b, c, d, a, 8, 20, 28);
            G(ref a, b, c, d, 13, 5, 29);
            G(ref d, a, b, c, 2, 9, 30);
            G(ref c, d, a, b, 7, 14, 31);
            G(ref b, c, d, a, 12, 20, 32);

            /* Round 3 */
            H(ref a, b, c, d, 5, 4, 33);
            H(ref d, a, b, c, 8, 11, 34);
            H(ref c, d, a, b, 11, 16, 35);
            H(ref b, c, d, a, 14, 23, 36);
            H(ref a, b, c, d, 1, 4, 37);
            H(ref d, a, b, c, 4, 11, 38);
            H(ref c, d, a, b, 7, 16, 39);
            H(ref b, c, d, a, 10, 23, 40);
            H(ref a, b, c, d, 13, 4, 41);
            H(ref d, a, b, c, 0, 11, 42);
            H(ref c, d, a, b, 3, 16, 43);
            H(ref b, c, d, a, 6, 23, 44);
            H(ref a, b, c, d, 9, 4, 45);
            H(ref d, a, b, c, 12, 11, 46);
            H(ref c, d, a, b, 15, 16, 47);
            H(ref b, c, d, a, 2, 23, 48);

            /* Round 4 */
            I(ref a, b, c, d, 0, 6, 49);
            I(ref d, a, b, c, 7, 10, 50);
            I(ref c, d, a, b, 14, 15, 51);
            I(ref b, c, d, a, 5, 21, 52);
            I(ref a, b, c, d, 12, 6, 53);
            I(ref d, a, b, c, 3, 10, 54);
            I(ref c, d, a, b, 10, 15, 55);
            I(ref b, c, d, a, 1, 21, 56);
            I(ref a, b, c, d, 8, 6, 57);
            I(ref d, a, b, c, 15, 10, 58);
            I(ref c, d, a, b, 6, 15, 59);
            I(ref b, c, d, a, 13, 21, 60);
            I(ref a, b, c, d, 4, 6, 61);
            I(ref d, a, b, c, 11, 10, 62);
            I(ref c, d, a, b, 2, 15, 63);
            I(ref b, c, d, a, 9, 21, 64);

            a = a + aa;
            b = b + bb;
            c = c + cc;
            d = d + dd;
        }

        private byte[] CreatePaddedBuffer(byte[] mes)
        {
            var padSize = 448 - mes.Length * 8 % 512;


            var pad = (uint)((padSize + 512) % 512);
            if (pad == 0) pad = 512;

            var sizeMsgBuff = (uint)(mes.Length + pad / 8 + 8);
            var sizeMsg = (ulong)mes.Length * 8;
            var bMsg = new byte[sizeMsgBuff];

            for (var i = 0; i < mes.Length; i++) bMsg[i] = mes[i];

            bMsg[mes.Length] |= 0x80;

            for (var i = 8; i > 0; i--) bMsg[sizeMsgBuff - i] = (byte)((sizeMsg >> ((8 - i) * 8)) & 0x00000000000000ff);
            return bMsg;
        }

        private void CopyBlock(byte[] bMsg, uint block)
        {
            block = block << 6;
            for (uint j = 0; j < 61; j += 4)
            {
                _x[j >> 2] = ((uint)bMsg[block + j + 3] << 24) |
                             ((uint)bMsg[block + j + 2] << 16) |
                             ((uint)bMsg[block + j + 1] << 8) |
                             bMsg[block + j];
            }
        }

        #region Helper

        private sealed class Digest
        {
            public uint A;
            public uint B;
            public uint C;
            public uint D;

            public Digest()
            {
                A = 0x67452301;
                B = 0xEFCDAB89;
                C = 0x98BADCFE;
                D = 0X10325476;
            }

            public override string ToString()
            {
                string st;
                st = BitHelper.ReverseByte(A).ToString("x8") +
                     BitHelper.ReverseByte(B).ToString("x8") +
                     BitHelper.ReverseByte(C).ToString("x8") +
                     BitHelper.ReverseByte(D).ToString("x8");
                return st;
            }
        }

        private static class BitHelper
        {
            /// <summary>
            ///     rotate
            /// </summary>
            /// <param name="num">num</param>
            /// <param name="shift">shift</param>
            /// <returns></returns>
            public static uint RotateLeft(uint num, ushort shift)
            {
                return (num >> (32 - shift)) | (num << shift);
            }

            /// <summary>
            ///     reverse
            /// </summary>
            public static uint ReverseByte(uint num)
            {
                return ((num & 0x000000ff) << 24) |
                       (num >> 24) |
                       ((num & 0x00ff0000) >> 8) |
                       ((num & 0x0000ff00) << 8);
            }
        }

        #endregion Helper

        #region Transform

        private void F(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + BitHelper.RotateLeft(a + ((b & c) | (~b & d)) + _x[k] + T[i - 1], s);
        }

        private void G(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + BitHelper.RotateLeft(a + ((b & d) | (c & ~d)) + _x[k] + T[i - 1], s);
        }

        private void H(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + BitHelper.RotateLeft(a + (b ^ c ^ d) + _x[k] + T[i - 1], s);
        }

        private void I(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + BitHelper.RotateLeft(a + (c ^ (b | ~d)) + _x[k] + T[i - 1], s);
        }

        #endregion Transform
    }
}
