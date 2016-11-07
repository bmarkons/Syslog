using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace User
{
    public class UserProxy : ChannelFactory<IPayments>, IPayments, IDisposable
    {
        IPayments factory;

        public UserProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool Payment1()
        {
            bool success = false;
            try
            {
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
