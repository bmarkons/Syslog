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

        public void SendAll(List<Log> logList)
        {
            if (ReplicatorClient.IsConnected)
            {
                ReplicatorClient.LogList.AddRange(logList);
            }

            using (StreamWriter file = new StreamWriter(FILENAME, true))
            {
                foreach (Log log in logList)
                {
                    file.WriteLine(log.ToString());
                }
            }

            Console.WriteLine("{0} new logs", logList.Count);
        }
    }
}
