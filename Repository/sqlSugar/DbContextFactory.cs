using ServiceExt;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DbContextFactory
    {
        private static readonly string connString = Appsettings.GetSectionValue("ConnectionString:Entities");
        //private static readonly string slaveconnString = ConfigurationManager.ConnectionStrings["slaveEntities"].ToString();

        public static  SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connString,// "server=.;uid=sa;pwd=@jhl85661501;database=SqlSugar4XTest",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsShardSameThread = true
                //SlaveConnectionConfigs = new List<SlaveConnectionConfig>() {//从连接
                //     new SlaveConnectionConfig() { HitRate=10, ConnectionString=slaveconnString }
                //} //主从数据库开放使用
            });
            //Print sql
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }

    }
}
