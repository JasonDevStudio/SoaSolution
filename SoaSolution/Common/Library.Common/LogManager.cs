using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace Library.Common
{
    public class LogManager
    {
        private static string logPath = string.Empty;
        private static bool _bWriteLog = true;
        private static readonly object m_lock = new object();


        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    //if (System.Web.HttpContext.Current == null)
                    // // Windows Forms 应用
                    // logPath = AppDomain.CurrentDomain.BaseDirectory;
                    //else
                    // Web 应用
                    logPath = AppDomain.CurrentDomain.BaseDirectory + @"log\";
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                }
                return logPath;
            }
            set
            {
                logPath = value;
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
            }
        }
        public static bool bWriteLog
        {
            get
            {
                return _bWriteLog;
            }
            set
            {
                _bWriteLog = value;
            }
        }

        /// <summary>
        /// 写跟踪日志
        /// </summary>
        public static void WriteTraceLog(string strMsg)
        {
            lock (m_lock)
            {
                if (_bWriteLog)
                {
                    try
                    {
                        string strTmp;
                        StackTrace st = new StackTrace(1, true);
                        //Rmp.Common.ConsoleEx.WriteLine(" Stack trace for current level: {0}", st.ToString());
                        StackFrame sf = st.GetFrame(0);
                        strTmp = "文件路径[" + sf.GetFileName() + "],  ";
                        strTmp += "函数名[" + sf.GetMethod().Name + "],  ";
                        strTmp += "代码行[" + sf.GetFileLineNumber() + "]";

                        System.IO.StreamWriter sw = System.IO.File.AppendText(LogPath + "Train" + " " + DateTime.Now.ToString("yyyyMMdd") + ".Log");
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]: "));
                        sb.Append("调试信息:" + strMsg + "    " + strTmp);
                        sw.WriteLine(sb.ToString());
                        sw.Close();
                    }
                    catch (System.IO.IOException IoEx)
                    {
                        LogManager.WriteErrorLog(IoEx.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        public static void WriteErrorLog(string strError)
        {
            lock (m_lock)
            {
                if (_bWriteLog)
                {
                    try
                    {
                        string strTmp;
                        StackTrace st = new StackTrace(1, true);
                        //Rmp.Common.ConsoleEx.WriteLine(" Stack trace for current level: {0}", st.ToString());
                        StackFrame sf = st.GetFrame(0);
                        strTmp = "文件路径[" + sf.GetFileName() + "],  ";
                        strTmp += "函数名[" + sf.GetMethod().Name + "],  ";
                        strTmp += "代码行[" + sf.GetFileLineNumber() + "]";

                        System.IO.StreamWriter sw = System.IO.File.AppendText(LogPath + "Error" + " " + DateTime.Now.ToString("yyyyMMdd") + ".Log");
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]: "));
                        sb.Append("错误信息:" + strError + "    " + strTmp);
                        sw.WriteLine(sb.ToString());
                        sw.Close();
                    }
                    catch //(System.IO.IOException IoEx)
                    {
                        //Common.LogManager.WriteErrorLog(IoEx.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 写警告日志
        /// </summary>
        public static void WriteWarningLog(string strError)
        {
            lock (m_lock)
            {
                if (_bWriteLog)
                {
                    try
                    {
                        string strTmp;
                        StackTrace st = new StackTrace(1, true);
                        //Rmp.Common.ConsoleEx.WriteLine(" Stack trace for current level: {0}", st.ToString());
                        StackFrame sf = st.GetFrame(0);
                        strTmp = "文件路径[" + sf.GetFileName() + "],  ";
                        strTmp += "函数名[" + sf.GetMethod().Name + "],  ";
                        strTmp += "代码行[" + sf.GetFileLineNumber() + "]";

                        System.IO.StreamWriter sw = System.IO.File.AppendText(LogPath + "Warn" + " " + DateTime.Now.ToString("yyyyMMdd") + ".Log");
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]: "));
                        sb.Append("警告信息:" + strError + "    " + strTmp);
                        sw.WriteLine(sb.ToString());
                        sw.Close();
                    }
                    catch (System.IO.IOException IoEx)
                    {
                        LogManager.WriteErrorLog(IoEx.Message);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }
}
