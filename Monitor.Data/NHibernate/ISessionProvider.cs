using Monitor.Data.Config;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using Monitor.Data.Types;
using Monitor.ComponentModel;

namespace Monitor.Data
{
    public interface ISessionProvider
    {
        string ConnectionString { get; }
        bool SessionFactoryCacheEnabled { get; }
        IConfigurationProvider ConfigProvider { get; }
        ISessionFactory SessionFactory { get; }
        DataBaseType DataBaseType { get; }
        string DefaultSchema { get; }

        ISessionFactory BuildSessionFactory();
        void Configure(IServiceCollection services, string configPath);
        void Configure(IConfigurationProvider configurationProvider);
        string GetClassMappingTable(Entidade entity);
        ISession GetCurrentSession();
        ISession OpenSession();
        IStatelessSession OpenStatelessSession();
        void UpdateSchema();
    }
}
