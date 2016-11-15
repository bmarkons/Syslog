using Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ServiceModel;
using CertificateManager;

namespace LoggingManager
{
    public abstract class Logger
    {
        private List<Log> logList = new List<Log>();
        private object locker = new object();
        private NetTcpBinding binding;
        private EndpointAddress address;

        public Logger()
        {
            Thread thread = new Thread(new ThreadStart(WaitAndSend));
            thread.Start();

            binding = new NetTcpBinding();

            X509Certificate2 srvCert = CertificateManager.CertificateManager.GetCertificateFromFile("SyslogService.cer");
            //3. korak sa table
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            address = new EndpointAddress(new Uri("net.tcp://localhost:514/SyslogService"),
                                          new X509CertificateEndpointIdentity(srvCert));
        }

        private void WaitAndSend()
        {
            while (true)
            {
                lock (locker)
                {
                    if (logList.Count != 0)
                    {
                        using (SyslogProxy proxy = new SyslogProxy(binding, address))
                        {
                            proxy.SendAll(logList);
                            logList.Clear();
                        }
                    }
                }
                Thread.Sleep(3000);
            }
        }

        public abstract void AuthorizationSuccess(string userName, string serviceName);

        public abstract void AuthenticationSuccess(string userName);

        public abstract void AuthorizationFailed(string userName, string serviceName, string reason);

        protected abstract void Log(string logMessage);

        protected void AddToLogList(int facCode, int sevLvl, string timestamp, int procId, string host, string msg)
        {
            lock (locker)
            {
                Log log = new Log()
                {
                    FacilityCode = facCode,
                    SeverityLevel = sevLvl,
                    Timestamp = timestamp,
                    ProcessId = procId,
                    Hostname = host,
                    Message = msg
                };

                logList.Add(log);
            }
        }
    }
}
