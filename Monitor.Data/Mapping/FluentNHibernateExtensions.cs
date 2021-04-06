using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;

namespace Monitor.Data.Mapping
{
    public static class FluentNHibernateExtensions
    {
        public static FluentMappingsContainer AddFromAssemblyList(
            this FluentMappingsContainer fmc, List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetExportedTypes()
                    .Where(x => IsMappingOf<IMappingProvider>(x) ||
                                IsMappingOf<IIndeterminateSubclassMappingProvider>(x) ||
                                IsMappingOf<IExternalComponentMappingProvider>(x) ||
                                IsMappingOf<IFilterDefinition>(x));

                foreach (Type t in types)
                {
                    fmc.Add(t);
                }
            }
            return fmc;
        }

        /// <summary>
        /// Private helper method cribbed from FNH source (PersistenModel.cs:151)
        /// </summary>
        private static bool IsMappingOf<T>(Type type)
        {
            return !type.IsGenericType && typeof(T).IsAssignableFrom(type);
        }
    }
}
