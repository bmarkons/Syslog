using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using Contracts;
using System.ServiceModel.Security;
using CertificateManager;

namespace Syslog
{
    class Program
    {
        static readonly string template = "net.tcp://{0}:{1}/SyslogService";

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            ServiceHost host = new ServiceHost(typeof(SyslogService));

            if (AppConfig.IS_MAIN_SYSLOG)
            {
                setCertificateAuthentication(binding, host);
            }
            else
            {
                setWindowsAuthentification();
            }

            host.AddServiceEndpoint(typeof(ISyslog), binding, string.Format(template, "localhost", AppConfig.MY_PORT));

            host.Open();
            Console.WriteLine("{0} started at {1}...", AppConfig.IS_MAIN_SYSLOG ? "Syslog service" : "Backup server", DateTime.Now);

            Console.ReadLine();
            host.Close();
        }

        private static void setWindowsAuthentification()
        {
            //throw new NotImplementedException();
        }

        private static void setCertificateAuthentication(NetTcpBinding binding, ServiceHost host)
        {
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            host.Credentials.ServiceCertificate.Certificate = CertificateManager.CertificateManager.GetCertificateFromFile("SyslogService.pfx", "ftn");
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
        }
    }
}
