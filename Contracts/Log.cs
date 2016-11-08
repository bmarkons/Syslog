using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Log
    {
        private int facilityCode;
        private int severityLevel;
        private int processId;
        private string timestamp;
        private string hostname;
        private string message;

        public int FacilityCode
        {
            get
            {
                return facilityCode;
            }

            set
            {
                facilityCode = value;
            }
        }

        public int SeverityLevel
        {
            get
            {
                return severityLevel;
            }

            set
            {
                severityLevel = value;
            }
        }

        public int ProcessId
        {
            get
            {
                return processId;
            }

            set
            {
                processId = value;
            }
        }

        public string Timestamp
        {
            get
            {
                return timestamp;
            }

            set
            {
                timestamp = value;
            }
        }

        public string Hostname
        {
            get
            {
                return hostname;
            }

            set
            {
                hostname = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        public override string ToString()
        {
            string str = string.Format("{0} {1} {2} {3} {4} {5}", timestamp, facilityCode, severityLevel, 
                processId, hostname, message);
            return str;
        }
    }
}
