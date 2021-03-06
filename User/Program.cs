﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;
using CertificateManager;

namespace User
{
	class Program
	{
		private static readonly int TEST = 1000;

		static void Main(string[] args)
		{
			NetTcpBinding binding = new NetTcpBinding();
			string template = "net.tcp://localhost:{0}/WCFComponent";

			int port = GetPort();

			if (port != 0)
			{
				string address = String.Format(template, port);
                X509Certificate2 srvCert = CertificateManager.CertificateManager.GetCertificateFromFile("WCFComponent.cer");
				//3. korak sa table
				binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
				EndpointAddress endpoint = new EndpointAddress(new Uri(address),
														  new X509CertificateEndpointIdentity(srvCert));
				//EndpointAddress endpoint = new EndpointAddress(new Uri(address));
				using (UserProxy proxy = new UserProxy(binding, endpoint))
				{
					for (int i = 0; i < TEST; i++)
					{
						proxy.Payment1();
						proxy.Payment2();
						Thread.Sleep(200);
						Console.Clear();
						float percent = (float) (i + 1)*100/TEST;
						Console.WriteLine("Completed : {0:N2} %", percent);
					}
				}

			}
            Console.ReadLine();

        }
		/// <summary>
		/// Show menu for choosing WCFComponent
		/// </summary>
		/// <returns>Port</returns>
        private static int GetPort()
		{
			do
			{
				Console.Clear();
				Console.WriteLine("-----------Choose WCFComponent-----------");
				Console.WriteLine("1. WCFComponent_1");
				Console.WriteLine("2. WCFComponent_2");
				Console.WriteLine("3. WCFComponent_3");
				Console.WriteLine("0. EXIT");

				var cond = Console.Read();

				switch (cond)
				{
					case '1':
						return 9997;
					case '2':
						return 9998;
					case '3':
						return 9999;
					case '0':
						return 0;
					default:
						continue;
				}

			} while (true);
		}
	}
}
