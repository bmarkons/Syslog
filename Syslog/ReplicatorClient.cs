using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;

namespace Syslog
{
    public class ReplicatorClient
    {
        private static readonly Thread _thread;
        private static ISyslog _backupService;
        private static ChannelFactory<ISyslog> factory;

        public static List<Log> LogList { get; set; }

        static ReplicatorClient()
        {
            LogList = new List<Log>();
            _thread = new Thread(new ThreadStart(WaitAndSend));
            _thread.Start();
        }

        public void CloseClient()
        {
            _thread.Abort();
        }

        private static void WaitAndSend()
        {
            while (true)
            {
                if (LogList.Count != 0 && IsConnected)
                {
                    if (_backupService != null)
                    {
                        _backupService.SendAll(LogList);
                        Console.WriteLine("{0} logs are sent to backup server.", LogList.Count);
                        LogList.Clear();
                    }
                }
                Thread.Sleep(1000);
            }
        }

        internal static void CreateChannel(string backupAddress)
        {
            ChannelFactory<ISyslog> factory = new ChannelFactory<ISyslog>(new NetTcpBinding(), backupAddress);
            _backupService = factory.CreateChannel();
        }

        public static bool IsConnected
        {
            get
            {
                if (factory == null)
                {
                    return false;
                }

                return factory.State == CommunicationState.Opened;
            }
        }
    }
}
