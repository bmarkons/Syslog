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
    public class Replicator
    {
        ISyslog backupProxy;
        ChannelFactory<ISyslog> factory;
        private readonly object locker = new object();
        private Thread replicator;
        private Queue<Log> logQueue = new Queue<Log>(100);
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
            factory = new ChannelFactory<ISyslog>(new NetTcpBinding(), string.Format("net.tcp://{0}:{1}/SyslogService", AppConfig.BACKUP_ADDRESS, AppConfig.BACKUP_PORT));
            backupProxy = factory.CreateChannel();
            replicator = new Thread(ReplicatePeriodically);
            replicator.Start();
        }

        private void ReplicatePeriodically()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (logQueue.Count != 0)
                {
                    Replicate();
                }
            }
        }

        private void Replicate()
        {
            try
            {
                lock (locker)
                {
                    backupProxy.SendAll(logQueue.ToList());
                    Console.WriteLine("Replicated {0} logs on backup server.", logQueue.Count);
                    logQueue.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Backup server not available : {0}.", e.GetType());
                backupProxy = factory.CreateChannel();
            }
        }

        public void EqueueLogs(List<Log> logList)
        {
            lock (locker)
            {
                foreach (Log log in logList)
                    logQueue.Enqueue(log);
            }
        }
    }
}

