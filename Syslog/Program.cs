using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using CertificateManager;

namespace Syslog
{
	class Program
	{
		public static readonly int PORT = 514;
		public static readonly int BACKUP = 513;

		static void Main(string[] args)
		{
			NetTcpBinding binding = new NetTcpBinding();
			string template = "net.tcp://localhost:{0}/SyslogService";

			string address = string.Format(template, PORT);

			if (PORT == 514)
			{
				string backupAddress = string.Format(template, BACKUP);

				ReplicatorClient.CreateChannel(backupAddress);
			}

			//need for certificate authentication
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


			ServiceHost host = new ServiceHost(typeof(SyslogService));
			host.AddServiceEndpoint(typeof(ISyslog), binding, address);

			//4. korak
			host.Credentials.ServiceCertificate.Certificate = CertificateManagerClass.GetCertificateFromFile("SyslogService.pfx","ftn");
			//5. korak
			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
			host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			host.Open();
			Console.WriteLine("Syslog service started at {0}...", DateTime.Now);

			Console.ReadLine();
			host.Close();
		}
	}
}
