using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
namespace Library.Common
{
    public class SerializerDeserialize
    {
        /// <summary>
        /// 序列化压缩
        /// </summary>
        /// <param name="obj">需要压缩的对象</param>
        /// <returns>Byte[]</returns>
        public static byte[] Serializer(object obj)
        {
            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化object对象
            MemoryStream ms = new MemoryStream();//创建内存流对象
            formatter.Serialize(ms, obj);//把object对象序列化到内存流
            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组
            ms.Close();//关闭内存流对象
            ms.Dispose();//释放资源
            MemoryStream msNew = new MemoryStream();
            GZipStream gzipStream = new GZipStream(msNew, CompressionMode.Compress, true);//创建压缩对象
            gzipStream.Write(buffer, 0, buffer.Length);//把压缩后的数据写入文件
            gzipStream.Close();//关闭压缩流,这里要注意：一定要关闭，要不然解压缩的时候会出现小于4K的文件读取不到数据，大于4K的文件读取不完整
            gzipStream.Dispose();//释放对象

            var bufferNew = msNew.ToArray();

            msNew.Close();
            msNew.Dispose();
            return bufferNew;
        }
        /// <summary>
        /// 反序列化 解压缩
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] bytes)
        {
            MemoryStream msNew = new MemoryStream(bytes);
            msNew.Position = 0;
            GZipStream gzipStream = new GZipStream(msNew, CompressionMode.Decompress);//创建解压对象
            byte[] buffer = new byte[4096];//定义数据缓冲
            int offset = 0;//定义读取位置
            MemoryStream ms = new MemoryStream();//定义内存流
            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                ms.Write(buffer, 0, offset);//解压后的数据写入内存流
            }
            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化object对象
            ms.Position = 0;//设置内存流的位置
            object obj;
            try
            {
                obj = (object)sfFormatter.Deserialize(ms);//反序列化
            }
            catch
            {
                throw;
            }
            finally
            {
                ms.Close();//关闭内存流
                ms.Dispose();//释放资源
            }
            gzipStream.Close();//关闭解压缩流
            gzipStream.Dispose();//释放资源
            msNew.Close();
            msNew.Dispose();
            return obj;
        }
    }
}