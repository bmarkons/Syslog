using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CertificateManager;
using Contracts;
using System.ServiceModel.Security;

namespace LoggingManager
{
    class SyslogProxy : ChannelFactory<ISyslog>, ISyslog, IDisposable
    {
        ISyslog factory;

        public SyslogProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
			//4. korak
			this.Credentials.ClientCertificate.Certificate = CertificateManager.CertificateManager.GetCertificateFromFile("WCFComponent.pfx","ftn");
			//5. korak
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			factory = this.CreateChannel();
        }

        public void SendAll(List<Log> logList)
        {
            try
            {
                factory.SendAll(logList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while sending logs to Syslog: {0}", ex.Message);
            }
        }
    }
}
