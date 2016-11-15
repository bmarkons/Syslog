using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ISyslog
    {
		/// <summary>
		/// Send all logs to the Syslog
		/// </summary>
		/// <param name="logList"> list of logs</param>
        [OperationContract]
        void SendAll(List<Log> logList);
	}
}
