using System;

namespace LoggingManager
{
	public class XMLLogger : ILogger
	{
		public void AuthenticationSuccess(string userName)
		{
			string msg = String.Format(AuditEvents.UserAuthenticationSuccess, userName);
			Console.WriteLine("[XMLLogger]: " + msg);
		}

		public void AuthorizationFailed(string userName, string serviceName, string reason)
		{
			string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
			Console.WriteLine("[XMLLogger]: " + msg);
		}

		public void AuthorizationSuccess(string userName, string serviceName)
		{
			string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
			Console.WriteLine("[XMLLogger]: " + msg);
		}
	}
}
