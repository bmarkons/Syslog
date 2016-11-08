using System;
using System.Diagnostics;

namespace LoggingManager
{
	public class WindowsEventLogger : Logger
	{
		public override void AuthenticationSuccess(string userName)
		{
			try
			{
				Audit.AuthenticationSuccess(userName);

			}
			catch (Exception e)
			{
				//Console.WriteLine(e);
			}

			string msg = String.Format(AuditEvents.UserAuthenticationSuccess, userName);
            Log(msg);
        }

        public override void AuthorizationFailed(string userName, string serviceName, string reason)
		{
			try
			{
				Audit.AuthorizationFailed(userName, serviceName, reason);
			}
			catch (Exception e)
			{
				//Console.WriteLine(e);
			}
			string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
            Log(msg);
        }

        public override void AuthorizationSuccess(string userName, string serviceName)
		{
			try
			{
				Audit.AuthorizationSuccess(userName,serviceName);
			}
			catch (Exception e)
			{
				//Console.WriteLine(e);
			}
			string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
			Log(msg);
		}

        protected override void Log(string logMessage)
        {
            string timestamp = DateTime.Now.ToString();
            
            AddToLogList(4, 1, timestamp, Process.GetCurrentProcess().Id, Environment.MachineName, logMessage);
        }
	}
}
