using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class CustomPrincipal
    {
        WindowsIdentity identity;
        HashSet<string> groups = new HashSet<string>();
        HashSet<string> permissions = new HashSet<string>();

        public CustomPrincipal(WindowsIdentity id)
            : base()
        {
            identity = id;

            foreach (var group in identity.Groups)
            {
                string groupName = GetGroupName(group);

                string[] s = RolesConfig.GetValue(groupName);

                if (s != null)
                    permissions.UnionWith(s);

                groups.Add(groupName);
            }
        }

        private string GetGroupName(IdentityReference group)
        {
            string fullname = group.Translate(typeof(SecurityIdentifier)).Translate(typeof(NTAccount)).Value;

            if (fullname.Contains("\\"))
            {
                string[] splitName = fullname.Split('\\');
                return splitName[1];
            }
            else if (fullname.Contains("@"))
            {
                string[] splitName = fullname.Split('@');
                return splitName[0];
            }

            return fullname;
        }

        public bool IsInRole(string role)
        {
            return permissions.Contains(role);
        }

        public IIdentity Identity
        {
            get { return identity; }
        }
    }
}
