using Contracts;
using System;
using System.Diagnostics;
using LoggingManager;

namespace WCFComponent
{
    public class WindowsEventLogger : Logger
    {
        private static readonly object locker = new object();

        public override void AuthenticationSuccess(string userName)
        {
            try
            {
                lock (locker)
                {
                    Audit.AuthenticationSuccess(userName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string msg = string.Format(AuditEvents.UserAuthenticationSuccess, userName);
            Log(msg);
            Console.WriteLine(msg);
        }

        public override void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            try
            {
                lock (locker)
                {
                    Audit.AuthorizationFailed(userName, serviceName, reason);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
            Log(msg);
            Console.WriteLine(msg);
        }

        public override void AuthorizationSuccess(string userName, string serviceName)
        {
            try
            {
                lock (locker)
                {
                    Audit.AuthorizationSuccess(userName, serviceName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
            Log(msg);
            Console.WriteLine(msg);
        }

        protected override void Log(string logMessage)
        {
            string timestamp = DateTime.Now.ToString();

            AddToLogList((int)FacilityKeyword.auth, 1, timestamp, Process.GetCurrentProcess().Id, Environment.MachineName, logMessage);
        }
    }
}
