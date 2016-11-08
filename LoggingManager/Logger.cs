using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LoggingManager
{
    public abstract class Logger
    {
        private List<Log> logList = new List<Log>();
        private object locker = new object();

        public abstract void AuthorizationSuccess(string userName, string serviceName);

        public abstract void AuthenticationSuccess(string userName);

        public abstract void AuthorizationFailed(string userName, string serviceName, string reason);

        protected abstract void Log(string logMessage);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="facCode">Facility code</param>
        /// <param name="sevLvl"></param>
        /// <param name="timestamp"></param>
        /// <param name="procId"></param>
        /// <param name=""></param>
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
