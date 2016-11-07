using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace WCFComponent
{
    class Program
    {
        static void Main(string[] args)
        {
            //create hosts
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFComponent";
            ServiceHost host = new ServiceHost(typeof(WCFComponentService1));
            host.AddServiceEndpoint(typeof(IPayments), binding, address);

            //Open hosts
            host.Open();
            Console.WriteLine("WCFComponentService1 started @ {0}", DateTime.Now);
            Console.ReadLine();

            host.Close();
        }
    }
}
