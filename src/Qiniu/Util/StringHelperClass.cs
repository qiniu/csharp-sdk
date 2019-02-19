//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2015 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------

public static class StringHelperClass
{
     public static string SubstringSpecial(this string self, int start, int end)
    {
        return self.Substring(start, end - start);
    }

    public static bool StartsWith(this string self, string prefix, int toffset)
    {
        return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
    }

    public static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
    {
        string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

        if (trimTrailingEmptyStrings)
        {
            if (splitArray.Length > 1)
            {
                for (int i = splitArray.Length; i > 0; i--)
                {
                    if (splitArray[i - 1].Length > 0)
                    {
                        if (i < splitArray.Length)
                            System.Array.Resize(ref splitArray, i);

                        break;
                    }
                }
            }
        }

        return splitArray;
    }


    public static string NewString(byte[] bytes)
    {
        return NewString(bytes, 0, bytes.Length);
    }
    public static string NewString(byte[] bytes, int index, int count)
    {
        return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
    }
    public static string NewString(byte[] bytes, string encoding)
    {
        return NewString(bytes, 0, bytes.Length, encoding);
    }
    public static string NewString(byte[] bytes, int index, int count, string encoding)
    {
        return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
    }


    public  static byte[] GetBytes(this string self)
    {
        return GetbytesForEncoding(System.Text.Encoding.UTF8, self);
    }
    public  static byte[] GetBytes(this string self, string encoding)
    {
        return GetbytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
    }
    public static byte[] GetbytesForEncoding(System.Text.Encoding encoding, string s)
    {
        byte[] bytes = new byte[encoding.GetByteCount(s)];
        encoding.GetBytes(s, 0, s.Length, (byte[])(object)bytes, 0);
        return bytes;
    }
}
