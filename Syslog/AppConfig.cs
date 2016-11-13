using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syslog
{
    public static class AppConfig
    {
        public static readonly bool IS_MAIN_SYSLOG = Convert.ToBoolean(ConfigurationSettings.AppSettings["isMainSyslog"]);
        public static readonly int MY_PORT = Convert.ToInt32(ConfigurationSettings.AppSettings["myPort"]);
        public static readonly string BACKUP_ADDRESS = ConfigurationSettings.AppSettings["backupAddress"];
        public static readonly int BACKUP_PORT = Convert.ToInt32(ConfigurationSettings.AppSettings["backupPort"]);
        public static readonly bool HAS_BACKUP = Convert.ToBoolean(ConfigurationSettings.AppSettings["hasBackup"]);
    }
}
