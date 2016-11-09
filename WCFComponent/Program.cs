using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Contracts;
using Security;
using LoggingManager;
using CertificateManager;

namespace WCFComponent
{
	class Program
	{
        private static readonly string XML_FILENAME = "logs.xml";
        private static readonly string TEXT_FILENAME = "logs.txt";

        static void Main(string[] args)
		{
			NetTcpBinding binding = new NetTcpBinding();
			string address1 = "net.tcp://localhost:9997/WCFComponent";
			string address2 = "net.tcp://localhost:9998/WCFComponent";
			string address3 = "net.tcp://localhost:9999/WCFComponent";

			TextLogger textLogger = new TextLogger(TEXT_FILENAME);
			WindowsEventLogger windowsEventLogger = new WindowsEventLogger();
			XMLLogger xmlLogger = new XMLLogger(XML_FILENAME);

			//create hosts
			ServiceHost host1 = new ServiceHost(typeof(WCFComponentService));
			ServiceHost host2 = new ServiceHost(typeof(WCFComponentService));
			ServiceHost host3 = new ServiceHost(typeof(WCFComponentService));

			//3. korak sa table
			//binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			host1.AddServiceEndpoint(typeof(IPayments), binding, address1);
			host2.AddServiceEndpoint(typeof(IPayments), binding, address2);
			host3.AddServiceEndpoint(typeof(IPayments), binding, address3);

			host1.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(textLogger);
			host2.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(windowsEventLogger);
			host3.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(xmlLogger);

			SetupSecurity(host1, textLogger);
			SetupSecurity(host2, windowsEventLogger);
			SetupSecurity(host3, xmlLogger);

			//setting for windoweventlogger
			ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
			newAudit.AuditLogLocation = AuditLogLocation.Application;
			newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
			newAudit.SuppressAuditFailure = true;

			host2.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
			host2.Description.Behaviors.Add(newAudit);

			host2.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
			host2.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

			//Open hosts
			host1.Open();
			host2.Open();
			host3.Open();

			Console.WriteLine("WCFComponentService1 started @ {0}", DateTime.Now);
			Console.WriteLine("WCFComponentService2 started @ {0}", DateTime.Now);
			Console.WriteLine("WCFComponentService3 started @ {0}", DateTime.Now);

			Console.ReadLine();

			host1.Close();
			host2.Close();
			host3.Close();

		}

		private static void SetupSecurity(ServiceHost host, Logger textLogger)
		{
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy(textLogger));
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

			////4. korak
			//host.Credentials.ServiceCertificate.Certificate = CertificateManagerClass.GetCertificateFromFile("WCFComponent.pfx","ftn");
			////5. korak
			//host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
			//host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
		}
	}
}
