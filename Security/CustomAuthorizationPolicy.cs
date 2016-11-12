using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LoggingManager;

namespace Security
{
	public class CustomAuthorizationPolicy: IAuthorizationPolicy
	{
		private Logger logger;
		private string id;
		private object locker = new object();

		public CustomAuthorizationPolicy(Logger logger)
		{
			this.logger = logger;
			this.id = Guid.NewGuid().ToString();
		}

		public bool Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			object list;

			if (!evaluationContext.Properties.TryGetValue("Identities", out list))
			{
				return false;
			}

			IList<IIdentity> identities = list as IList<IIdentity>;
			if (list == null || identities.Count <= 0)
			{
				return false;
			}

            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0]);
			return true;
		}

		protected virtual IPrincipal GetPrincipal(IIdentity identity)
		{
			lock (locker)
			{
                CustomPrincipal principal = null;

                if (identity.AuthenticationType == "X509")
				{

                    principal = new CustomPrincipal(identity);
                    logger.AuthenticationSuccess(principal.Name);
				}

				return principal;
			}
		}

		public ClaimSet Issuer
		{
			get { return ClaimSet.System; }
		}

		public string Id
		{
			get { return this.id; }
		}
	}
}
