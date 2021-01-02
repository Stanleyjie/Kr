using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows.Service
{
   static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //调试使用
            //Application.Run(new TestForm());

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TimeService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
