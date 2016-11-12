using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using LoggingManager;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Security
{
	public class CustomAuthorizationManager : ServiceAuthorizationManager
	{
		private Logger logger;

		public CustomAuthorizationManager(Logger logger)
		{
			this.logger = logger;
		}
		protected override bool CheckAccessCore(OperationContext operationContext)
		{
			//check and log
			bool authorized = false;

            //IIdentity identity = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["MyIdentity"] as IIdentity;
            //Type x509Type = identity.GetType();
            //FieldInfo cerField = x509Type.GetField("certificate", BindingFlags.Instance | BindingFlags.NonPublic);

            //X509Certificate2 c = cerField.GetValue(identity) as X509Certificate2;

            CustomPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal;

			var serviceName = operationContext.IncomingMessageHeaders.Action;
			if (principal != null)
			{
				var userName = principal.Name;
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
