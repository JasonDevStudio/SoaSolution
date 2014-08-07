using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace System.Data
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Created by：cz，2014-5-21，DataTable转换成IList（类型中的属性名需和字段名一致）
        /// </summary>
        /// <typeparam name="T">类 型</typeparam>
        /// <param name="dt">DataTable 本身</param>
        /// <returns>返回 IList</returns>
        public static IList<T> ToList<T>(this DataTable dt) where T : class,new()
        {
            IList<T> result = new List<T>(); 

            List<PropertyInfo> listOfPropertyInfos = new List<PropertyInfo>();
            Type t = typeof(T);

            //获得T的所有的Public属性，并找出T属性和DataTable的列名称相同的属性(PropertyInfo)，加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p =>
            {
                if (dt.Columns.IndexOf(p.Name) != -1)
                {
                    listOfPropertyInfos.Add(p);
                }
            }
            );
            foreach (DataRow row in dt.Rows)
            {
                T temp = new T();
                listOfPropertyInfos.ForEach(p =>
                {
                    if (row[p.Name] != DBNull.Value)
                    {
                        p.SetValue(temp, row[p.Name], null);
                    }
                }
                );
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// DataTable 分页
        /// </summary>
        /// <param name="dataInput">输入Table</param> 
        /// <param name="pageIndex">输入-当前索引页</param>
        /// <param name="pageSize">输入-每页显示行数</param>
        /// <returns>DataTable</returns>
        public static DataTable ToPager(this DataTable dataInput,  int pageIndex = 1, int pageSize = 5)
        { 
            if (dataInput == null || dataInput.Rows.Count < 1)
            { 
                return dataInput;
            }

            // 输出表数据
            var dtOut = dataInput.Clone();

            // 数据总数
            var rowCount = dataInput.Rows.Count;
             
            // 开始取值行索引
            var rowStartIndex = (pageIndex - 1) * pageSize + 1;

            // 结束取值行索引
            var rowEndIndex = rowCount > pageSize * pageIndex ? pageSize * pageIndex : rowCount;

            for (int i = rowStartIndex; i <= rowEndIndex; i++)
            {
                dtOut.ImportRow(dataInput.Rows[i-1]);
            }

            return dtOut;
        }

        /// <summary>
        /// DataTable 获取分页总页数
        /// </summary>
        /// <param name="dataInput">数据源</param> 
        /// <param name="pageSize">每页显示行数</param>
        /// <returns>总页数</returns>
        public static int GetTotalPages(this DataTable dataInput,int pageSize = 5)
        {
            if (dataInput == null || dataInput.Rows.Count < 1)
            {
                return 0; 
            }

            // 数据总数
            var rowCount = dataInput.Rows.Count;

            // 总页数
            var totalPages = rowCount % pageSize > 0 ? rowCount / pageSize + 1 : rowCount / pageSize;

            return totalPages;
        }
    }
    
}

namespace System
{
    /// <summary>
    /// 时间扩展函数类
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 需要过滤的时间集合
        /// </summary>
        private static DateTime[] FilterTime = { DateTime.MinValue, DateTime.MaxValue, DateTime.Parse("1900-01-01 00:00:00") };
        
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="datetime">时间源</param>
        /// <param name="format">格式化 公式 </param>
        /// <returns>String</returns>
        public static string ToFilter(this DateTime datetime,string format="yyyy-MM-dd")
        { 
            for (int i = 0; i < FilterTime.Length; i++)
            {
                if (datetime == FilterTime[i])
                {
                    return string.Empty;
                }
            }

            var strDateTime = string.Format("{0:" + format + "}", datetime);

            return strDateTime; 
        }
    }

    /// <summary>
    /// 字符串扩展函数类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="obj">需要截取的字符串</param>
        /// <param name="length">长度</param>
        /// <returns>字符串</returns>
        public static string ToSubstringByLength(this string obj, int length)
        {
            if (!string.IsNullOrWhiteSpace(obj))
            {
                if (obj.Length > length)
                {
                    return obj.Substring(0, length) + "...";
                }
                else
                {
                    return obj;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 格式化 
        /// </summary>
        /// <param name="obj">需要处理的对象</param>
        /// <param name="format">格式化 公式</param>
        /// <returns>字符串</returns>
        public static string ToFormat(this object obj, string format = null)
        {
            if (obj != null &&  ! string.IsNullOrWhiteSpace(obj.ToString()))
            {
                if (string.IsNullOrWhiteSpace(format))
                {
                    return obj.ToString();
                }

                var value = string.Format("{0:"+format+"}",obj);

                return value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}