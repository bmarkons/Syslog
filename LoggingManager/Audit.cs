using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingManager
{
	public class Audit: IDisposable
	{
		private static EventLog customLog = null;
		const string SourceName = "LoggingManager";
		const string LogName = "WCFEventLog";

		static Audit()
		{
			try
			{
				if (!EventLog.SourceExists(SourceName))
				{
					EventLog.CreateEventSource(SourceName, LogName);
				}

				customLog = new EventLog(LogName, Environment.MachineName, SourceName);
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}


		public static void AuthenticationSuccess(string userName)
		{
			// string UserAuthenticationSuccess -> read string format from .resx file
			if (customLog != null)
			{
				string msg = String.Format(AuditEvents.UserAuthenticationSuccess, userName);
				// string message -> create message based on UserAuthenticationSuccess and params
				// write message in customLog, EventLogEntryType is Information or SuccessAudit 
				customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthenticationSuccess));
			}
		}

		public static void AuthorizationSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
				customLog.WriteEntry(msg, EventLogEntryType.SuccessAudit);

			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthorizationSuccess));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
		/// <param name="reason">permission name</param>
		public static void AuthorizationFailed(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
				customLog.WriteEntry(msg, EventLogEntryType.FailureAudit);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UserAuthorizationFailed));
			}
		}

		public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
