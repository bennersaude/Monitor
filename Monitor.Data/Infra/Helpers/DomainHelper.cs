using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monitor.Data.Infra.Helpers
{
    public sealed class DomainHelper
    {
        public static Assembly GetAssembly(string assemblyName)
        {
            if (String.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentNullException("assemblyName");

            var cleanName = assemblyName.ToLower().Replace(".dll", String.Empty);

            return GetLoadedAssembly(cleanName) ?? LoadAssembly(cleanName);
        }

        private static Assembly GetLoadedAssembly(string name)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(
                        x => String.Equals(x.GetName().Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        private static Assembly LoadAssembly(string name)
        {
            return AppDomain.CurrentDomain.Load(name);
        }

        private static IEnumerable<Assembly> GetLoadedBennerAssembly()
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(
                        x => x.GetName().Name.ToLower().Contains("benner"));
        }
    }
}
