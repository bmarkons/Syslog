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

namespace User
{
    public class UserProxy : ChannelFactory<IPayments>, IPayments, IDisposable
    {
        IPayments factory;

        public UserProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            ////4. korak
            this.Credentials.ClientCertificate.Certificate = CertificateManager.CertificateManager.GetCertificateFromFile("WCFClient2.pfx", "ftn");
            ////5. korak
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            factory = this.CreateChannel();
        }

        public bool Payment1()
        {
            bool success = false;
            try
            {
                var v = this.State;
                success = factory.Payment1();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while calling payment1: {0}.", ex.Message);
            }

            return success;
        }

        public bool Payment2()
        {
            bool success = false;
            try
            {
                success = factory.Payment2();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while calling payment2: {0}.", ex.Message);
            }

            return success;
        }
    }
}
