using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

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

            return true;
        }
    }
}
