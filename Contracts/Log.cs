using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public enum FacilityKeyword
    {
        kern = 0,
        user = 1,
        mail = 2,
        deamon = 3,
        auth = 4,
        syslog = 5,
        authpriv = 10
        //add more if need
    }

    [DataContract]
    public class Log
    {
        private int facilityCode;
        private int severityLevel;
        private int processId;
        private string timestamp;
        private string hostname;
        private string message;

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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
