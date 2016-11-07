using System;

namespace LoggingManager
{
	public class WindowsEventLogger : ILogger
	{
		public void AuthenticationSuccess(string userName)
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
			Console.WriteLine("[WindowsEventLogger]: " + msg);
		}

		public void AuthorizationFailed(string userName, string serviceName, string reason)
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
			Console.WriteLine("[WindowsEventLogger]: " + msg);
		}

		public void AuthorizationSuccess(string userName, string serviceName)
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
			Console.WriteLine("[WindowsEventLogger]: " + msg);
		}
	}
}
