using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Library.Common
{
    public class CommonMethod
    {
        /// <summary> 
        /// 图片上传
        /// </summary> 
        public static string ImageUpload(out ResultBase result, HttpContextBase cxt)
        {
            var fileName = string.Empty;
            result = new ResultBase();
            result.resultMsg = string.Empty;
            result.result = 1;

            HttpPostedFileBase file = null;
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };         //文件允许格式
            if (cxt.Request.Files.Count < 1 || cxt.Request.Files[0] == null || string.IsNullOrWhiteSpace(cxt.Request.Files[0].FileName))
            {
                result.result = -1;
                result.resultMsg = "未接收到文件!";
                return fileName;
            }

            file = cxt.Request.Files[0];
            string Extension = Path.GetExtension(file.FileName);
            if (Array.IndexOf(filetype, Extension) > -1)
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Extension;
                string upPath = cxt.Server.MapPath("~/Uploads/Images/");
                file.SaveAs(upPath + fileName);
            }
            else
            {
                result.result = -2;
                result.resultMsg = "上传的文件不是图片格式,请重新上传!";

            }

            return fileName;
        }

        /// <summary>
        /// 字符串截取
        /// </summary> 
        public static string ObjSubstringByLength(string obj,int length)
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
        /// 移除数字后面无效的零
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveZero(string value)
        {
            if (value.Split('.').Length == 2)
            {
                string result = value.Split('.')[0];
                string strChar = value.Split('.')[1];
                strChar.ToCharArray();
                for (int i = strChar.Length - 1; i >= 0; i--)
                {
                    if (strChar[i] == 0 || strChar[i] == '0')
                    {
                        strChar = strChar.Remove(i);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(strChar.ToString()))
                {
                    return result + "." + strChar.ToString();
                }
                else
                {
                    return result;
                }
            }

            return value;
        }
         
    }
}
