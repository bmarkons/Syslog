using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class RolesConfig
    {
        private static ResourceManager resourceManager = null;
        private static ResourceSet resourceSet = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceManager
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(RolesConfigFile).FullName, Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        private static ResourceSet ResourceSet
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceSet == null)
                    {
                        resourceSet = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                    }
                    return resourceSet;
                }
            }
        }

        public static string[] GetValue(string rolename)
        {
            string value = ResourceManager.GetString(rolename);

            string[] prms = value?.Split(',');
            return prms;
        }

        public static void PrintRBACModel()
        {
            Console.WriteLine("***PRINTING ROLES WITH PERMISSIONS**");
            foreach (DictionaryEntry entry in ResourceSet)
            {
                Console.WriteLine("ROLE: {0} => PRMS: {1}", entry.Key.ToString(), entry.Value.ToString());
            }
            Console.WriteLine("***********************************");
        }
    }
}
