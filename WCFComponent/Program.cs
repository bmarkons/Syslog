using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using Security;

namespace WCFComponent
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address1 = "net.tcp://localhost:9997/WCFComponent";
            string address2 = "net.tcp://localhost:9998/WCFComponent";
            string address3 = "net.tcp://localhost:9999/WCFComponent";

            //create hosts
            ServiceHost host1 = new ServiceHost(typeof(WCFComponentService));
            ServiceHost host2 = new ServiceHost(typeof(WCFComponentService));
            ServiceHost host3 = new ServiceHost(typeof(WCFComponentService));

            host1.AddServiceEndpoint(typeof(IPayments), binding, address1);
            host2.AddServiceEndpoint(typeof(IPayments), binding, address2);
            host3.AddServiceEndpoint(typeof(IPayments), binding, address3);

            host1.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(new TextLogger());
            host1.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(new WindowsEventLogger());
            host1.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager(new XMLLogger());

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
    }
}
