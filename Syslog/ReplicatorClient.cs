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


		public static List<Log> LogList { get; set; }

		static ReplicatorClient()
		{
			IsConnected = false;
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
					_backupService?.SendAll(LogList);
					LogList.Clear();
				}
				Thread.Sleep(1000);
			}

		}

		internal static void CreateChannel(string backupAddress)
		{
			ChannelFactory<ISyslog> factory = new ChannelFactory<ISyslog>(new NetTcpBinding(), backupAddress);
			_backupService = factory.CreateChannel();
			IsConnected = true;
		}

		public static bool IsConnected { get; set; }
	}
}
