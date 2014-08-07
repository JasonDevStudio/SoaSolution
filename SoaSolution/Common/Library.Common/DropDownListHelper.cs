using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Mvc;
using Library.StringItemDict;

namespace Library.Common
{
    /// <summary>
    /// Created by；cz，2014-5-21，下拉框辅助类
    /// </summary>
    public class DropDownListHelper
    {
        #region 通用
        /// <param name="dtSource"></param>
        /// <param name="valueFieldName"></param>
        /// <param name="textFieldNames">多个用英文半角逗号隔开</param>
        /// <param name="selectedValue"></param>
        /// <param name="isAllowedEmpty">是否允许空选项</param>
        /// <param name="spliter">只有textFieldNames为多个时才起作用，用于分割显示文本</param>
        /// <param name="isFirstSelected"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GenerateItems(DataTable dtSource,
            string valueFieldName, string textFieldNames, string selectedValue = null,
            bool isAllowedEmpty = false, string spliter = null, bool isFirstSelected = false)
        {
            var results = new List<SelectListItem>();

            if (dtSource != null && dtSource.Rows.Count > 0)
            {

                var textFields = textFieldNames.Split(SymbolCharConstsDict.Comma);//获取textFields
                int index = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    var value = dr[valueFieldName].ToString();
                    var text = string.Empty;
                    foreach (string s in textFields)
                    {
                        if (text.Length > 0)
                        {
                            text += spliter;
                        }
                        text += dr[s].ToString();
                    }

                    var listItem = new SelectListItem()
                    {
                        Text = text,
                        Value = value
                    };

                    if (!string.IsNullOrWhiteSpace(selectedValue) && dr[valueFieldName].ToString() == selectedValue)//设置选中值
                    {
                        listItem.Selected = true;
                    }
                    else if (isFirstSelected && index == 0)
                    {
                        listItem.Selected = true;
                    }
                    results.Add(listItem);

                    index++;
                }
            }

            if (isAllowedEmpty)//是否允许空选项
            {

                results.Insert(0, new SelectListItem() { Text = "全部", Value = string.Empty });

            }

            return results;
        }

        /// <param name="typeName">类名称（注意：内部类的调用方式  ContainerClassName+InternalClassName）</param>
        /// <param name="selectedValue"></param>
        /// <param name="isAllowedEmpty">是否允许空选项</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GenerateItemsFromStringItems(string typeName,
            string selectedValue = null, bool isAllowedEmpty = false)
        {
            var results = new List<SelectListItem>();

            var stringItems = StringItem.GetTypeItems(typeName);
            if (stringItems != null && stringItems.Length > 0)
            {
                int index = 0;
                foreach (StringItem si in stringItems)
                {
                    var value = si.Code;
                    var text = si.Name;

                    var listItem = new SelectListItem()
                    {
                        Text = text,
                        Value = value
                    };

                    if (!string.IsNullOrWhiteSpace(selectedValue) && value == selectedValue)//设置选中值
                    {
                        listItem.Selected = true;
                    }

                    if (string.IsNullOrWhiteSpace(selectedValue) && !isAllowedEmpty && index == 0) //如果不允许为空，则默认选中第一项
                    {
                        listItem.Selected = true;
                    }

                    results.Add(listItem);

                    index++;
                }
            }

            if (isAllowedEmpty)//是否允许空选项
            {
                results.Insert(0, new SelectListItem() { Text = string.Empty, Value = string.Empty });
            }

            return results;
        }

        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="valueFieldName">多个用英文半角逗号隔开</param>
        /// <param name="textFieldNames">多个用英文半角逗号隔开</param>
        /// <param name="isAllowedEmpty">是否允许空选项</param>
        /// <param name="spliter">只有textFieldNames为多个时才起作用，用于分割显示文本</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GenExpandItems(DataTable dtSource,
            string valueFieldName, string textFieldNames, string selectedValue = null,
            bool isAllowedEmpty = false, string spliter = null, bool isFirstSelected = false)
        {
            var results = new List<SelectListItem>();

            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                var textFields = textFieldNames.Split(SymbolCharConstsDict.Comma);//获取textFields
                var valueFiles = valueFieldName.Split(SymbolCharConstsDict.Comma);//获取valueFiles

                int index = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    var value = string.Empty;
                    foreach (string v in valueFiles)
                    {
                        if (value != string.Empty) { value += spliter; }
                        value += dr[v].ToString();
                    }

                    var text = string.Empty;
                    foreach (string s in textFields)
                    {
                        if (text.Length > 0)
                        {
                            text += spliter;
                        }
                        text += dr[s].ToString();
                    }

                    var listItem = new SelectListItem()
                    {
                        Text = text,
                        Value = value
                    };

                    if (!string.IsNullOrWhiteSpace(selectedValue) && dr[valueFieldName].ToString() == selectedValue)//设置选中值
                    {
                        listItem.Selected = true;
                    }
                    else if (isFirstSelected && index == 0)
                    {
                        listItem.Selected = true;
                    }
                    results.Add(listItem);

                    index++;
                }
            }

            if (isAllowedEmpty)//是否允许空选项
            {
                results.Insert(0, new SelectListItem() { Text = string.Empty, Value = string.Empty });
            }

            return results;
        }



        #endregion
    }
}
