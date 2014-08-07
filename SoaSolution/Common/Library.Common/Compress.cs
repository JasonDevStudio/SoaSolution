using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Library.Common
{
    public class Compress
    { 
        public static byte[] Zip(byte[] sourceBytes)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                GZipStream gStream = new GZipStream(mStream, CompressionMode.Compress);
                gStream.Write(sourceBytes, 0, sourceBytes.Length);
                gStream.Close();
                return mStream.ToArray();
            }
        }

        public static byte[] UnZip(byte[] sourceBytes)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (GZipStream gStream = new GZipStream(new MemoryStream(sourceBytes), CompressionMode.Decompress))
                {
                    int readBytes = 0;
                    byte[] buffer = new byte[1024];
                    while ((readBytes = gStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        mStream.Write(buffer, 0, readBytes);
                    }
                    return mStream.ToArray();
                }
            }
        }
    }
}
