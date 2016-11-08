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
        [DataMember]
        public int FacilityCode { get; set; }

        [DataMember]
        public int SeverityLevel { get; set; }

		[DataMember]
        public int ProcessId { get; set; }

		[DataMember]
        public string Timestamp { get; set; }

		[DataMember]
        public string Hostname { get; set; }

		[DataMember]
        public string Message { get; set; }

		public override string ToString()
        {
            string str = string.Format("{0} {1} {2} {3} {4} {5}", Timestamp, FacilityCode, SeverityLevel,
                ProcessId, Hostname, Message);
            return str;
        }
    }
}
