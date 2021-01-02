using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Windows.Service
{
    partial class TimeService : ServiceBase
    {
        //定时器
        protected System.Timers.Timer severTime;

        //同步锁
        private static object snycRoot = new object();

        //服务名
        protected string serviceName = ConfigurationManager.AppSettings["serviceName"] ?? "定时服务_XX";

        //循环时间3540000
        protected int Initialtime = int.Parse(ConfigurationManager.AppSettings["Initialtime"] ?? "1800000");

        //请求地址
        private string apiUrl = ConfigurationManager.AppSettings["apiUrl"] ?? "";

        private WebUtils hClient = new WebUtils();

        public TimeService()
        {
            InitializeComponent();
            InitService();
        }

        //初始化
        private void InitService()
        {
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            this.ServiceName = serviceName;
            this.AutoLog = false;//使用自定义日志时，必须将 AutoLog 设置为 false

            severTime = new System.Timers.Timer();
            severTime.Elapsed += new ElapsedEventHandler(Business);
            severTime.Interval = Initialtime;
            severTime.AutoReset = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.severTime.Enabled = true;
                this.severTime.Start();
                Business(null, null);
                LogHelper.Info(this.serviceName, "已成功启动");
            }
            catch (Exception ex)
            {
                LogHelper.Error(this.serviceName, "OnStart错误:" + ex.Message);
            }
            // TODO: 在此处添加代码以启动服务。
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            try
            {
                this.severTime.Stop();
                LogHelper.Info(serviceName, "已停止");
            }
            catch (Exception ex)
            {
                LogHelper.Info(serviceName, "OnStop错误:" + ex.Message);
            }
        }

        //暂停了
        protected override void OnContinue()
        {
            this.severTime.Start();
            base.OnContinue();
            LogHelper.Info(serviceName, "已暂停启动");
        }

        //继续
        protected override void OnPause()
        {
            this.severTime.Stop();
            base.OnPause();
            LogHelper.Info(serviceName, "已继续启动");
        }


        /// <summary>
        /// 业务处理
        /// </summary>
        protected void Business(object sender, ElapsedEventArgs e)
        {
            lock (snycRoot)
            {
                try
                {
                    //if (DateTime.Now.Hour == 2 && DateTime.Now.Minute > 30)
                    //{
                    //    LogHelper.Info("同步数据", hClient.DoPost(apiUrl + "/MyYuyue/synchro", new Dictionary<string, string>(), "UTF-8"));
                    //}
                    //else
                    //{
                    //    LogHelper.Info("同步数据", "同步信息日志");
                    //}

                    if (GetTaskStatus("t1"))
                    {
                        //hClient.DoGet(apiUrl + "/api/common/AsnycInfo", new Dictionary<string, string>(), "UTF-8");
                        LogHelper.Info("同步信息成功", "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateTaskStatus("t1");
                    }

                    if (GetTaskStatus("t2"))
                    {
                        //hClient.DoGet(apiUrl + "/api/common/AsnycInfo", new Dictionary<string, string>(), "UTF-8");
                        LogHelper.Info("同步信息成功", "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateTaskStatus("t2");
                    }

                    if (GetTaskStatus("t3"))
                    {
                        //hClient.DoGet(apiUrl + "/api/common/AsnycInfo", new Dictionary<string, string>(), "UTF-8");
                        LogHelper.Info("同步信息成功", "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateTaskStatus("t3");
                    }

                    if (GetTaskStatus("t4"))
                    {
                        //hClient.DoGet(apiUrl + "/api/common/AsnycInfo", new Dictionary<string, string>(), "UTF-8");
                        LogHelper.Info("同步信息成功", "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        UpdateTaskStatus("t4");
                    }

                    //处理超时订单
                    hClient.DoPost(apiUrl + "/api/HisOrderApi/ClearTimeOutHisOrder", new Dictionary<string, string>(), "UTF-8");
                    LogHelper.Info("处理超时订单", "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                }
                catch (Exception ex)
                {
                    LogHelper.Error("业务处理", JsonConvert.SerializeObject(ex));
                }
            }
        }


        #region 测试使用
        public void testStart()
        {
            this.OnStart(null);
            //GetTaskStatus("t1");
        }

        public void testStop()
        {
            this.OnStop();
            //UpdateTaskStatus("t0");
        }
        #endregion

        #region 本地文件
        public bool GetTaskStatus(string key)
        {
            try
            {
                using (FileStream fs = new FileStream(path: System.AppDomain.CurrentDomain.BaseDirectory + "/TaskConfig.json",
                        mode: FileMode.OpenOrCreate, access: FileAccess.ReadWrite, share: FileShare.ReadWrite))
                {
                    byte[] bytes = new byte[fs.Length];
                    List<TaskConfigModel> list = new List<TaskConfigModel>();
                    if (fs.Length <= 0)
                    {

                        for (int i = 0; i < 11; i++)
                        {
                            list.Add(new TaskConfigModel() { Key = $"t{i}", Status = 0, OperTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") });
                        }
                        string jsonTxt = JsonConvert.SerializeObject(list);
                        bytes = Encoding.Default.GetBytes(jsonTxt);
                        fs.Write(bytes, 0, bytes.Length);
                        return true;
                    }
                    else
                    {
                        StreamReader sr = new StreamReader(fs);
                        string jsonText = sr.ReadToEnd();
                        list = JsonConvert.DeserializeObject<List<TaskConfigModel>>(jsonText);
                        var res = list.FirstOrDefault(x => x.Key == key);
                        sr.Close();
                        sr.Dispose();
                        if (res != null && res.OperTime != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            return true;
                        }
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                LogHelper.Error($"{key}任务失败", e.Message);
                return false;
            }

        }
        public void UpdateTaskStatus(string key)
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "/TaskConfig.json";
                string jsonText = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(jsonText))
                {
                    List<TaskConfigModel> list = new List<TaskConfigModel>();
                    list = JsonConvert.DeserializeObject<List<TaskConfigModel>>(jsonText);
                    var res = list.FirstOrDefault(x => x.Key == key);
                    res.Status = 1;
                    res.OperTime = DateTime.Now.ToString("yyyy-MM-dd");
                    jsonText = JsonConvert.SerializeObject(list);
                    File.WriteAllText(path, jsonText);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

    }

        public class TaskConfigModel
    {
        public string Key { get; set; }
        public int Status { get; set; }
        public string OperTime { get; set; }
    }
}
