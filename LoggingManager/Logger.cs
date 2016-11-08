using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.ServiceModel;

namespace LoggingManager
{
    public abstract class Logger
    {
        private List<Log> logList = new List<Log>();
        private object locker = new object();

        public Logger()
        {
            Thread thread = new Thread(new ThreadStart(WaitAndSend));
            thread.Start();
        }

        private void WaitAndSend()
        {
            while (true)
            {
                if (logList.Count != 0)
                {
                    string address = "net.tcp://localhost:514/SyslogService";
                    NetTcpBinding binding = new NetTcpBinding();
                    using (SyslogProxy proxy = new SyslogProxy(binding, new EndpointAddress(new Uri(address))))
                    {
                        proxy.SendAll(logList);
                        logList.Clear();
                    }
                }
                Thread.Sleep(3000);
            }
        }

        public abstract void AuthorizationSuccess(string userName, string serviceName);

        public abstract void AuthenticationSuccess(string userName);

        public abstract void AuthorizationFailed(string userName, string serviceName, string reason);

        protected abstract void Log(string logMessage);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="facCode">Facility code</param>
        /// <param name="sevLvl">Severity Level</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="procId">Process id</param>
        /// <param name="msg">Message</param>
        protected void AddToLogList(int facCode, int sevLvl, string timestamp, int procId, string host, string msg)
        {
            lock (locker)
            {
                Log log = new Log()
                {
                    FacilityCode = facCode,
                    SeverityLevel = sevLvl,
                    Timestamp = timestamp,
                    ProcessId = procId,
                    Hostname = host,
                    Message = msg
                };

                logList.Add(log);
            }
        }
    }
}
