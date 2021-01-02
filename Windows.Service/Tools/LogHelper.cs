using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Windows.Service
{
    /// <summary>
    /// 日志
    /// </summary>
    public sealed class LogHelper
    {
        //在网站根目录下创建日志目录
        private static string path = Application.StartupPath.ToString() + "/" + System.Configuration.ConfigurationManager.AppSettings["logPath"] ?? "logs";
        private static int LOG_LEVENL = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LOG_LEVENL"] ?? "1");
        private static object locker = new object();

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            if (LOG_LEVENL >= 3)
            {
                Task.Factory.StartNew(() =>
                    WriteLog("DEBUG", className, content)
                );
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            if (LOG_LEVENL >= 2)
            {
                Task.Factory.StartNew(() =>
                    WriteLog("INFO", className, content)
                );
            }
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            if (LOG_LEVENL >= 1)
            {
                Task.Factory.StartNew(() =>
                    WriteLog("ERROR", className, content)
                );
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        private static void WriteLog(string type, string className, string content)
        {
            try
            {
                if (!Directory.Exists(path + "/" + type))//如果日志目录不存在就创建
                {
                    Directory.CreateDirectory(path+ "/"+type);
                }

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string date_dir = time.Substring(0, 7);
                if (!Directory.Exists(path + "/" + type + "/" + date_dir))
                {
                    Directory.CreateDirectory(path + "/" + type+ "/" + date_dir);
                }

                string filename = path + "/" + type + "/" + date_dir + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

                //创建或打开日志文件，向日志文件末尾追加记录 using 自动释放

                //锁住
                lock (locker)
                {
                    using (StreamWriter mySw = File.AppendText(filename))
                    {
                        //向日志文件写入内容
                        string write_content = string.Format("[{0}] | 日志类型:{1} | Path:{2} | Info:{3}  \r\n ",
                                                                time, type, className, content);
                        mySw.WriteLine(write_content);

                        //关闭日志文件
                        mySw.Close();
                        mySw.Dispose();
                    }
                }

            }
            catch (Exception e)
            {
                var errmsg = e.Message;
                //不做处理
                return;
            }
        }
    }
}
