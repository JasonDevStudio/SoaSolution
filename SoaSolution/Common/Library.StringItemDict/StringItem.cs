using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;

namespace Library.StringItemDict
{
    /// <summary>
    /// string 类型的Item对象。
    /// </summary>
    public class StringItem
    {
        #region public property

        private readonly string m_Code = null;
        private readonly string m_Name = null;
        private readonly string m_NameEng = null;

        /// <summary>
        /// 编号（string 类型）
        /// </summary>
        public string Code
        {
            get
            {
                return m_Code;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public string NameEng
        {
            get
            {
                return m_NameEng;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建string 类型的Item对象
        /// </summary>
        /// <param name="Code">编号</param>
        public StringItem(string Code)
        {
            this.m_Code = Code;
            this.m_Name = "";
            this.m_NameEng = "";
        }

        /// <summary>
        /// 创建string 类型的Item对象
        /// </summary>
        /// <param name="Code">编号</param>
        /// <param name="Name">名称</param>
        public StringItem(string Code, string Name)
        {
            this.m_Code = Code;
            this.m_Name = Name;
            this.m_NameEng = "";
        }

        public StringItem(string Code, string Name, string NameEng)
        {
            this.m_Code = Code;
            this.m_Name = Name;
            this.m_NameEng = NameEng;
        }

        #endregion

        #region public method

        /// <summary>
        /// 设置默认返回属性

        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static implicit operator string(StringItem item)
        {
            return item.Code;
        }

        /// <summary>
        /// 将此实例的数值转换为它的等效字符串表示形式

        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Code;
        }

        #endregion public method

        #region Helper Methods

        private const string nameSpacePrefix = "Library.StringItemDict.";

        public static string GetNameByCode<T>(string code) where T : BaseDict
        {
            return StringItem.GetNameByCode(typeof(T), code);
        }

        public static string GetNameEngByCode<T>(string code) where T : BaseDict
        {
            return StringItem.GetNameEngByCode(typeof(T), code);
        }

        public static string GetNameDependonLanguage<T>(string code) where T : BaseDict
        {
            string languageName = Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            if (languageName == "en")
            {
                return StringItem.GetNameEngByCode(typeof(T), code);
            }
            else
            {
                return StringItem.GetNameByCode(typeof(T), code);
            }
        }

        public static Type GetDictType(string typeName)
        {
            return Type.GetType(nameSpacePrefix + typeName);
        }

        /// <summary>
        /// 获取指定类型的全部定义字段
        /// </summary>
        /// <param name="TypeName">要获取的类的名称</param>
        /// <returns>字段集合</returns>
        public static object[] GetTypeItems(string TypeName)
        {
            Type type = Type.GetType(nameSpacePrefix + TypeName);
            if (type == null)
            {
                return null;
            }
            return GetTypeItems(type);
        }

        /// <summary>
        /// 获取指定类型的全部定义字段
        /// </summary>
        /// <param name="type">要获取的类的类型</param>
        /// <returns>字段集合</returns>
        public static object[] GetTypeItems(System.Type type)
        {
            ArrayList items = new ArrayList();
            FieldInfo[] fis = type.GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (fi.FieldType.Equals(typeof(StringItem)))
                {
                    items.Add(fi.GetValue(null));
                }
            }
            return items.ToArray();
        }

        /// <summary>
        /// 根据指定的编码获取对应类型的名称
        /// </summary>
        /// <param name="TypeName">要获取的类的名称</param>
        /// <param name="Code">指定的编码</param>
        /// <returns>对应的名称</returns>
        public static string GetNameByCode(string TypeName, string Code)
        {
            Type type = Type.GetType(nameSpacePrefix + TypeName);
            return GetNameByCode(type, Code);
        }


        /// </summary>
        /// <param name="TypeName"></param>
        /// <param name="objectCode"></param>
        /// <returns></returns>
        public static string GetNameByCode(string TypeName, object objectCode)
        {
            if (objectCode == null) { return string.Empty; }
            return StringItem.GetNameByCode(TypeName, objectCode.ToString());
        }

        /// <summary>
        /// 根据指定的编码获取对应类型的名称
        /// </summary>
        /// <param name="type">要获取的类的类型</param>
        /// <param name="Code">指定的编码</param>
        /// <returns>对应的名称</returns>
        public static string GetNameByCode(Type type, string Code)
        {
            string Name = string.Empty;
            object[] items = GetTypeItems(type);
            foreach (StringItem item in items)
            {
                if (item.Code.Equals(Code))
                {
                    Name = item.Name;
                }
            }
            return Name;
        }

        public static string GetNameEngByCode(string TypeName, string Code)
        {
            Type type = Type.GetType(nameSpacePrefix + TypeName);
            return GetNameEngByCode(type, Code);
        }


        public static string GetNameEngByCode(Type type, string Code)
        {
            string Name = string.Empty;
            object[] items = GetTypeItems(type);
            foreach (StringItem item in items)
            {
                if (item.Code.Equals(Code))
                {
                    Name = item.NameEng;
                    break;
                }
            }

            return Name;
        }

        public static string GetNameDependonLanguage(Type type, string code)
        {
            string languageName = Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            if (languageName == "en")
            {
                return GetNameEngByCode(type, code);
            }
            else
            {
                return GetNameByCode(type, code);
            }
        }

        #endregion Helper Methods
    }
}
