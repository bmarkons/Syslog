using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace LoggingManager
{
    class SyslogProxy : ChannelFactory<ISyslog>, ISyslog, IDisposable
    {
        ISyslog factory;

        public SyslogProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void SendAll(List<Log> logList)
        {
            try
            {
                factory.SendAll(logList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while sending logs to Syslog: {0}", ex.Message);
            }
        }
    }
}
