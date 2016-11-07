namespace LoggingManager
{
	public interface ILogger
	{
		void AuthorizationSuccess(string userName, string serviceName);

		void AuthenticationSuccess(string userName);

		void AuthorizationFailed(string userName, string serviceName, string reason);
	}
}
