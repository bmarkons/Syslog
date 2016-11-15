using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.IO;

namespace Syslog
{
    public class SyslogService : ISyslog
    {
        private static readonly string FILENAME = "syslogs.txt";
        private static readonly object locker = new object();

        public void SendAll(List<Log> logList)
        {
            if (AppConfig.HAS_BACKUP)
            {
                Replicator.Instance.EqueueLogs(logList);
            }

            lock (locker)
            {
                using (StreamWriter file = new StreamWriter(FILENAME, true))
                {
                    foreach (Log log in logList)
                    {
                        file.WriteLine(log.ToString());
                    }
                }
            }

            Console.WriteLine("{0} new logs", logList.Count);
        }
    }
}
