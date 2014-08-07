using Library.Criterias.SoaTest;
using Library.Kernel.DataBaseHelper;
using Library.Logic.SoaTest.Interfaces;
using Library.Models.SoaTest;
using Library.StringItemDict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Library.Logic.SoaTest.Classes
{
    public class LogicTestUser : ILogicTestUser
    {
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="resultMsg">执行结果</param> 
        /// <param name="criteria">查询条件</param>
        /// <returns>泛型</returns>
        public IList<Models.SoaTest.ModelTestuser> QueryTestuserList(out string resultMsg, CriteriaTestuser criteria)
        {
            resultMsg = string.Empty;
            IList<ModelTestuser> list = new List<ModelTestuser>();
            try
            {
                //存储过程名称
                string sql = "SELECT  *  FROM [TestUser]  WHERE (@uname IS NULL OR [UName] =@uname)";

                //参数添加
                IList<DBParameter> parm = new List<DBParameter>();
                parm.Add(new DBParameter() { ParameterName = "uname", ParameterValue = criteria.UName, ParameterInOut = BaseDict.ParmIn, ParameterType = DbType.String }); 

                //查询执行
                using (IDataReader dr = DBHelper.ExecuteReader(sql, false, parm))
                {
                    //DataReader 转换成 List
                    list = GetModel(dr); 
                }
            }
            catch (Exception ex)
            {
                resultMsg = string.Format("{0} {1}", BaseDict.ErrorPrefix, ex.ToString());
            }

            return list;
        }

        #region 私有函数
        /// <summary>
        /// Model 赋值 IDataReader
        /// </summary>
        private IList<ModelTestuser> GetModel(IDataReader dr)
        {
            var modelList = new List<ModelTestuser>();

            while (dr.Read())
            {
                var model = new ModelTestuser();
                model.Uid = dr["UId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UId"]);
                model.Uname = dr["UName"] == DBNull.Value ? string.Empty : dr["UName"].ToString();
                model.Upwd = dr["UPwd"] == DBNull.Value ? string.Empty : dr["UPwd"].ToString();
                model.Uemail = dr["UEmail"] == DBNull.Value ? string.Empty : dr["UEmail"].ToString();
                model.Uphone = dr["UPhone"] == DBNull.Value ? string.Empty : dr["UPhone"].ToString();
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// Model 赋值 DataSet
        /// </summary>
        private IList<ModelTestuser> GetModel(DataSet ds)
        {
            var modelList = (from DataRow dr in ds.Tables[0].Rows
                             select new ModelTestuser()
                             {
                                 Uid = dr["UId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UId"]),
                                 Uname = dr["UName"] == DBNull.Value ? string.Empty : dr["UName"].ToString(),
                                 Upwd = dr["UPwd"] == DBNull.Value ? string.Empty : dr["UPwd"].ToString(),
                                 Uemail = dr["UEmail"] == DBNull.Value ? string.Empty : dr["UEmail"].ToString(),
                                 Uphone = dr["UPhone"] == DBNull.Value ? string.Empty : dr["UPhone"].ToString(),
                             }).ToList();
            return modelList;
        }

        #endregion

    }
}
