using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IPayments
    {
        [OperationContract]
        bool Payment1();

        [OperationContract]
        bool Payment2();
    }
}
