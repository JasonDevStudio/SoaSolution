using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using Library.StringItemDict;
using System.Configuration;

namespace Library.Kernel.DataBaseHelper
{
    public class DBHelper
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string DBNAME
        {
            get
            {
                return ConfigurationManager.AppSettings["DataAccess"].ToString(); 
            }
        }

        /// <summary>
        /// 临时使用这里产生一个其他库连接的事务对象
        /// </summary>
        /// <param name="dbName">数据库连接名称</param>
        /// <returns>事物</returns>
        public static DbTransaction OpenTransaction(string dbName = null)
        {
            Database db = null;
            if (string.IsNullOrWhiteSpace(dbName))
                db = DatabaseFactory.CreateDatabase(DBNAME);
            else
                db = DatabaseFactory.CreateDatabase(dbName);
            DbConnection con = db.CreateConnection();
            con.Open();
            return con.BeginTransaction();
        }


        /// <summary>
        /// 事物提交
        /// </summary>
        /// <param name="trans">事物</param>
        public static void Commit(DbTransaction trans)
        {
            // trans.Connection 在commit或rollback完成之后会被置为null，
            // 但对应的连接对象没有被关闭，有可能仍被连接池引用着，又没有关闭连接，而得不到释放
            DbConnection con = trans.Connection;
            trans.Commit();

            try
            {
                con.Close();
                con.Dispose();
            }
            finally { }
        }

        /// <summary>
        /// 事物回滚
        /// </summary>
        /// <param name="trans">事物</param>
        public static void RollBack(DbTransaction trans)
        {
            // trans.Connection 在commit或rollback完成之后会被置为null，
            // 但对应的连接对象没有被关闭，有可能仍被连接池引用着，又没有关闭连接，而得不到释放
            DbConnection con = trans.Connection;
            trans.Rollback();

            try
            {
                con.Close();
                con.Dispose();
            }
            finally { }
        }

        /// <summary>
        /// DataSet 查询
        /// </summary> 
        public static DataSet ExecuteDataSet<T>(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var ds = new DataSet();
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);

            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                ds = db.ExecuteDataSet(cmd, tran);

            }
            else
            {
                ds = db.ExecuteDataSet(cmd);
            }
            SetParameterValue(db, cmd, parm);
            return ds;
        }

        /// <summary>
        /// DataSet 查询
        /// </summary> 
        public static DataSet ExecuteDataSet(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var ds = new DataSet();
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);

            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                ds = db.ExecuteDataSet(cmd, tran);
            }
            else
            {
                ds = db.ExecuteDataSet(cmd);
            }
            SetParameterValue(db, cmd, parm);
            return ds;
        }

        /// <summary>
        /// 增、删、改 执行
        /// </summary> 
        public static int ExecuteNonQuery<T>(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            int exRes = 0;
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);

            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                exRes = db.ExecuteNonQuery(cmd, tran);
            }
            else
            {
                exRes = db.ExecuteNonQuery(cmd);
            }

            SetParameterValue(db, cmd, parm);
            return exRes;
        }

        /// <summary>
        /// 增、删、改 执行
        /// </summary> 
        public static int ExecuteNonQuery(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            int exRes = 0;
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);
            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                exRes = db.ExecuteNonQuery(cmd, tran);
            }
            else
            {
                exRes = db.ExecuteNonQuery(cmd);
            }

            SetParameterValue(db, cmd, parm);

            return exRes;
        }

        /// <summary>
        /// 查询所返回的结果集中第一行的第一列
        /// </summary> 
        public static object ExecuteScalar<T>(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);
            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                return db.ExecuteScalar(cmd, tran);
            }
            else
            {
                return db.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// 查询所返回的结果集中第一行的第一列
        /// </summary> 
        public static object ExecuteScalar(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);
            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }
            if (tran != null)
            {
                return db.ExecuteScalar(cmd, tran);
            }
            else
            {
                return db.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// DataReader 查询
        /// </summary> 
        public static IDataReader ExecuteReader<T>(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);
            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                return db.ExecuteReader(cmd, tran);
            }
            else
            {
                return db.ExecuteReader(cmd);
            }
        }

        /// <summary>
        /// DataReader 查询
        /// </summary> 
        public static IDataReader ExecuteReader(string sql, bool isStoredProc = false, IList<DBParameter> parm = null, DbTransaction tran = null)
        {
            var db = DatabaseFactory.CreateDatabase(DBNAME);
            var cmd = isStoredProc ? db.GetStoredProcCommand(sql) : db.GetSqlStringCommand(sql);
            if (parm != null)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            db.AddOutParameter(cmd, item.ParameterName, item.ParameterType, int.MaxValue);
                            break;
                        case BaseDict.ParmIn:
                        default:
                            db.AddInParameter(cmd, item.ParameterName, item.ParameterType, item.ParameterValue);
                            break;
                    }
                }
            }

            if (tran != null)
            {
                return db.ExecuteReader(cmd, tran);
            }
            else
            {
                return db.ExecuteReader(cmd);
            }
        }

        /// <summary>
        /// 返回参数赋值
        /// </summary> 
        private static void SetParameterValue(Database db, DbCommand cmd, IList<DBParameter> parm)
        {
            if (parm != null && parm.Count > 0)
            {
                foreach (var item in parm)
                {
                    switch (item.ParameterInOut.ToUpper())
                    {
                        case BaseDict.ParmOut:
                            item.ParameterValue = db.GetParameterValue(cmd, item.ParameterName);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 输出参数赋值
        /// </summary>
        /// <param name="parm">参数泛型</param>
        /// <param name="cmd"></param>
        public static void GetParameterValue(IList<DBParameter> parm, DbCommand cmd)
        {
            if (parm != null && parm.Count > 0)
            {
                foreach (var item in parm)
                {
                    if (item.ParameterInOut == BaseDict.ParmOut)
                    {
                        foreach (DbParameter tem in cmd.Parameters)
                        {
                            if (tem.Direction == ParameterDirection.Output)
                            {
                                item.ParameterValue = tem.Value;
                                break;
                            }
                        }
                    }
                }
            }
        }

    }
}
