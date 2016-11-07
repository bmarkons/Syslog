using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using LoggingManager;

namespace Security
{
	public class CustomAuthorizationManager : ServiceAuthorizationManager
	{
		private ILogger logger;

		public CustomAuthorizationManager(ILogger logger)
		{
			this.logger = logger;
		}
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
			//check and log
			bool authorized = false;

			IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

			var serviceName = operationContext.IncomingMessageHeaders.Action;
			if (principal != null)
			{
				var userName = principal.Identity.Name;
				authorized = principal.IsInRole("AccountUsers");

				if (authorized == false)
				{
					logger.AuthorizationFailed(userName, serviceName, "AccountUsers");
				}
				else
				{
					logger.AuthorizationSuccess(userName, serviceName);
				}
			}

			return true;
		}
	}
}
