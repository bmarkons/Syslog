using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
namespace WCFComponent
{
    public class WCFComponentService : IPayments
    {
        public bool Payment1()
        {
            Console.WriteLine("Payment1 executed.");
            return true;
        }

        public bool Payment2()
        {
            Console.WriteLine("Payment2 executed.");
            return true;
        }
    }
}
