﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace LoggingManager
{
    public class XMLLogger : Logger
    {
        private XmlDocument doc = new XmlDocument();
        private string logFileName;
        private object locker = new object();
        public XMLLogger(string logFileName)
        {
            this.logFileName = logFileName;
            if (File.Exists(logFileName))
            {
                doc.Load(logFileName);
            }
            else
            {
                doc.LoadXml("<logs></logs>");
            }
        }

        public override void AuthenticationSuccess(string userName)
        {
            string msg = String.Format(AuditEvents.UserAuthenticationSuccess, userName);
            Log(msg);
        }

        public override void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            string msg = String.Format(AuditEvents.UserAuthorizationFailed, userName, serviceName, reason);
            Log(msg);
        }

        public override void AuthorizationSuccess(string userName, string serviceName)
        {
            string msg = String.Format(AuditEvents.UserAuthorizationSuccess, userName, serviceName);
            Log(msg);
        }

        protected override void Log(string logMessage)
        {
            lock (locker)
            {
                string timestamp = DateTime.Now.ToString();

                XmlElement newLog = doc.CreateElement("log");
                XmlElement newTimestamp = doc.CreateElement("timestamp");
                XmlElement newMessage = doc.CreateElement("message");

                newTimestamp.InnerText = timestamp;
                newMessage.InnerText = logMessage;

                doc.DocumentElement.AppendChild(newLog);
                newLog.AppendChild(newTimestamp);
                newLog.AppendChild(newMessage);

                doc.Save(logFileName);
                AddToLogList(4, 1, timestamp, Process.GetCurrentProcess().Id, Environment.MachineName, logMessage);
            }
        }
    }
}
