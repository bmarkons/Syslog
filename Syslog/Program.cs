using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Syslog
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:514/SyslogService";

            ServiceHost host = new ServiceHost(typeof(SyslogService));
            host.AddServiceEndpoint(typeof(ISyslog), binding, address);

            host.Open();
            Console.WriteLine("Syslog service started at {0}...", DateTime.Now);

            Console.ReadLine();
            host.Close();
        }
    }
}
