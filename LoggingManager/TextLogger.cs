using System;
using System.Diagnostics;
using System.IO;

namespace LoggingManager
{
    public class TextLogger : Logger
    {
        private string logFileName;

        public TextLogger(string logFileName)
        {
            this.logFileName = logFileName;
        }

        public override void AuthenticationSuccess(string userName)
        {
            string msg = String.Format(AuditEvents.UserAuthenticationSuccess, userName);
            Log(msg);
        }

        public override void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
            Log(msg);
        }

        public override void AuthorizationSuccess(string userName, string serviceName)
        {
            string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
            Log(msg);
        }

        protected override void Log(string logMessage)
        {
            string timestamp = DateTime.Now.ToString();

            using (StreamWriter file = new StreamWriter(logFileName, true))
            {
                file.WriteLine(" " + timestamp + ": " + logMessage);
            }

            AddToLogList(4, 1, timestamp, Process.GetCurrentProcess().Id, Environment.MachineName, logMessage);
        }
    }
}
