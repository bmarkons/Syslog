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
            CustomPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            var serviceName = operationContext.IncomingMessageHeaders.Action;
            if (principal != null)
            {
                var userName = principal.Name;
                bool authorized = principal.IsInRole("AccountUsers");

                if (authorized == false)
                {
                    logger.AuthorizationFailed(userName, serviceName, "AccountUsers");
                    return false;
                }
                else
                {
                    logger.AuthorizationSuccess(userName, serviceName);
                }
            }
            else
            {
                logger.AuthorizationFailed("No principal", serviceName, "AccountUsers");
            }

            return true;
        }
    }
}
