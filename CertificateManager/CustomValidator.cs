using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateManager
{
	public class CustomValidator : X509CertificateValidator
	{
		public override void Validate(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
		{

		}
	}
}
