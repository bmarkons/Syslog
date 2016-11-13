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
        public static readonly int PORT = 514;
        public static readonly int BACKUP = 513;

        public static bool isBackup = Convert.ToBoolean(ConfigurationSettings.AppSettings["isBackup"]);

        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string template = "net.tcp://{0}:{1}/SyslogService";

            ServiceHost host = new ServiceHost(typeof(SyslogService));
            if (isBackup)
            {
                string address = string.Format(template, "localhost", BACKUP);
                host.AddServiceEndpoint(typeof(ISyslog), binding, address);

            }
            else
            {
                string address = string.Format(template, "localhost", PORT);
                string backupAddress = string.Format(template, "localhost", BACKUP);
                ReplicatorClient.CreateChannel(backupAddress);


                //need for certificate authentication
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


                host.AddServiceEndpoint(typeof(ISyslog), binding, address);

                //4. korak
                host.Credentials.ServiceCertificate.Certificate = CertificateManager.CertificateManager.GetCertificateFromFile("SyslogService.pfx", "ftn");
                //5. korak
                host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
                host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            }
            host.Open();
            Console.WriteLine("{0} started at {1}...", isBackup ? "Backup server" : "Syslog service", DateTime.Now);

            Console.ReadLine();
            host.Close();
        }
    }
}
