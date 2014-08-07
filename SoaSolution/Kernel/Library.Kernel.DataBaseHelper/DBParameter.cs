using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Library.Kernel.DataBaseHelper
{
    public class DBParameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object ParameterValue { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DbType ParameterType { get; set; }

        /// <summary>
        /// 参数输入输出类型
        /// </summary>
        public string ParameterInOut { get; set; }
    }
}
