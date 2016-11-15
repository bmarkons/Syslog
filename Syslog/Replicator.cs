using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.Threading;
using System.ServiceModel;

namespace Syslog
{
	/// <summary>
	/// Intermediary between Main Syslog and Backup
	/// </summary>
	public class Replicator
    {
        private ISyslog _backupProxy;
        private ChannelFactory<ISyslog> _factory;
        private readonly object locker = new object();
        private Thread _replicator;
        private Queue<Log> _logQueue = new Queue<Log>(100); //buffer
        private static Replicator instance;
        public static Replicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Replicator();
                }
                return instance;
            }
        }

        public Replicator()
        {
            _factory = new ChannelFactory<ISyslog>(new NetTcpBinding(), string.Format("net.tcp://{0}:{1}/SyslogService", AppConfig.BACKUP_ADDRESS, AppConfig.BACKUP_PORT));
            _backupProxy = _factory.CreateChannel();
            _replicator = new Thread(ReplicatePeriodically);
            _replicator.Start();
        }

		/// <summary>
		/// Wait for logs and replicate to the Backup server
		/// </summary>
        private void ReplicatePeriodically()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_logQueue.Count != 0)
                {
                    Replicate();
                }
            }
        }


		/// <summary>
		/// Trying to replicate logs from Queue. Create new channel, if channel is not created 
		/// </summary>
        private void Replicate()
        {
            try
            {
                lock (locker)
                {
                    _backupProxy.SendAll(_logQueue.ToList());
                    Console.WriteLine("Replicated {0} logs on backup server.", _logQueue.Count);
                    _logQueue.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Backup server not available : {0}.", e.GetType());
                _backupProxy = _factory.CreateChannel();
            }
        }

        public void EqueueLogs(List<Log> logList)
        {
            lock (locker)
            {
                foreach (Log log in logList)
                    _logQueue.Enqueue(log);
            }
        }
    }
}

