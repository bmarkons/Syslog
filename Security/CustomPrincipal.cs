using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class CustomPrincipal:IPrincipal
    {
        IIdentity identity;
        HashSet<string> groups = new HashSet<string>();
        public string Name { get; set; }


        public CustomPrincipal(IIdentity id)
            : base()
        {
            identity = id;
            string s = id.Name;
            if (s.Contains("OU"))
            {
                var ind = s.IndexOf("OU");
                string groupsPart = s.Substring(ind);
                groupsPart = groupsPart.Substring(0, groupsPart.IndexOf(';'));
                string groupsString = groupsPart.Split('=')[1];
                groupsString = groupsString.Substring(1, groupsString.Length - 2);
                var grupe = groupsString.Split('#');
                groups.UnionWith(grupe);
            }

            if (s.Contains("CN"))
            {
                int ind = s.IndexOf("CN");
                int idx;
                if(s.Contains(",")){
                    idx = s.IndexOf(",");
                }else{
                    idx = s.IndexOf(" ");
                }
                Name = s.Substring(ind, idx).Split('=')[1];
            }

        }


        public bool IsInRole(string groupName)
        {
            return groups.Contains(groupName);
        }

        public IIdentity Identity
        {
            get { return identity; }
        }
    }
}
