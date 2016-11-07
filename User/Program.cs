using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace User
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/WCFComponent";

            using (UserProxy proxy = new UserProxy(binding, new EndpointAddress(new Uri(address))))
            {
                proxy.Payment1();
                proxy.Payment2();
                // TODO: 
            }

            Console.ReadLine();
        }
    }
}
