using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace Library.Common
{
    /// <summary>
    /// 用于将数据导出到excel
    /// </summary>
    public class ExportToExcel
    { 

        #region 将DataTable导出到Excel  
        /// <summary>
        /// DataTable 导出Excel
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <param name="filepath">filepath</param>
        /// <param name="error">错误信息</param>
        /// <returns>bool</returns>
        public static bool DataTableToExcel(System.Data.DataTable datatable, string filepath, out string error)
        {
            error = "";
            try
            {
                if (datatable == null)
                {
                    error = "DataTableToExcel:datatable 为空";
                    return false;
                }

                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                Aspose.Cells.Worksheet sheet = workbook.Worksheets[0]; 
                Aspose.Cells.Cells cells = sheet.Cells;

                int nRow = 0;
                try
                {
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        cells[nRow, i].PutValue(datatable.Columns[i].ColumnName);
                        Aspose.Cells.Style style = new Aspose.Cells.Style();
                        style.Font.IsBold=true;
                        style.Font.Size = 11;
                        cells[nRow, i].SetStyle(style);
                    }
                }
                catch (System.Exception e)
                {
                    error = error + " DataTableToExcel: " + e.Message;
                } 

                foreach (DataRow row in datatable.Rows)
                {
                    nRow++;
                    try
                    {
                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            if (row[i].GetType().ToString() == "System.Drawing.Bitmap")
                            {
                                //------插入图片数据-------
                                System.Drawing.Image image = (System.Drawing.Image)row[i];
                                MemoryStream mstream = new MemoryStream();
                                image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                sheet.Pictures.Add(nRow, i, mstream);
                            }
                            else
                            {
                                cells[nRow, i].PutValue(row[i]);
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        error = error + " DataTableToExcel: " + e.Message;
                    }
                }
              
                workbook.Save(filepath);
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " DataTableToExcel: " + e.Message;
                return false;
            }
        }

        
        #endregion

        #region 导出到多张表 

        public static bool DataSetToExcel(System.Data.DataSet data, string filepath, out string error)
        {
            error = "";
            try
            {
                if (data == null || data.Tables.Count < 1)
                {
                    error = "DataTableToExcel:datatable 为空";
                    return false;
                }

                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                workbook.Worksheets.RemoveAt(0);
                for (int i = 0; i < data.Tables.Count; i++)
                {
                    var datatable = data.Tables[i];
                    Aspose.Cells.Worksheet sheet = workbook.Worksheets.Add(data.Tables[i].TableName);  
                    Aspose.Cells.Cells cells = sheet.Cells;
                      
                    int nRow = 0;
                    try
                    {
                        for (int j = 0; j < datatable.Columns.Count; j++)
                        {
                            cells[nRow,j].PutValue(datatable.Columns[j].ColumnName);
                            //Aspose.Cells.Style style = new Aspose.Cells.Style();
                            //style.Font.IsBold = true;
                            //style.Font.Size = 11;
                            //cells[nRow, j].SetStyle(style);
                        }
                    }
                    catch (System.Exception e)
                    {
                        error = error + " DataSetToExcel: " + e.Message;
                    }

                    foreach (DataRow row in datatable.Rows)
                    {
                        nRow++;
                        try
                        {
                            for (int j = 0; j < datatable.Columns.Count; j++)
                            { 
                                if (row[j].GetType().ToString() == "System.Drawing.Bitmap")
                                {
                                    //------插入图片数据-------
                                    System.Drawing.Image image = (System.Drawing.Image)row[j];
                                    MemoryStream mstream = new MemoryStream();
                                    image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    sheet.Pictures.Add(nRow, j, mstream);
                                }
                                else if (row[j].GetType().ToString() == "System.DateTime")
                                {
                                    var time = Convert.ToDateTime(row[j]); 
                                    if (time == DateTime.MinValue || time == DateTime.MaxValue || time == DateTime.Parse("1900-01-01 00:00:00"))
                                    {
                                        cells[nRow, j].PutValue(string.Empty);
                                    }
                                    else
                                    {
                                        cells[nRow, j].PutValue(time.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    
                                    //cells[nRow, j].PutValue(time);
                                }
                                else
                                {
                                    cells[nRow, j].PutValue(row[j]);
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            error = error + " DataSetToExcel: " + e.Message;
                        }
                    }
                     
                }

                workbook.Save(filepath);
                return true;
            }
            catch (System.Exception e)
            {
                error = error + " DataTableToExcel: " + e.Message;
                return false;
            }
        }

        #endregion
    }
}
