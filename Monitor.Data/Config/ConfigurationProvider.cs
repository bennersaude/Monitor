using NHibernate;
using System.Collections.Concurrent;

namespace Monitor.Data.Config
{
    public class ConfigurationProvider: IConfigurationProvider
    {
        public static string ConfigFilePath { get; set; }
        private ConcurrentDictionary<string, ISessionFactory> sessionFactories
            = new ConcurrentDictionary<string, ISessionFactory>();
        private ConcurrentDictionary<string, global::NHibernate.Cfg.Configuration> configurations
            = new ConcurrentDictionary<string, global::NHibernate.Cfg.Configuration>();

        private static readonly string DefaultConfigurationKey
            = typeof(global::NHibernate.Cfg.Configuration).FullName;
        private static readonly string DefaultSessionFactoryKey
            = typeof(ISessionFactory).FullName;

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactories.GetOrAdd(
                DefaultSessionFactoryKey,
                key =>
                {
                    var cfg = configurations[DefaultConfigurationKey];
                    if (cfg == null)
                    {
                        throw new System.InvalidOperationException(
                            "Default configuration does not exists!"
                        );
                    }
                    var sessionFactory = cfg.BuildSessionFactory();
                    return sessionFactory;
                }
            );
        }

        public ISessionFactory GetSessionFactory(string key)
        {
            return sessionFactories.GetOrAdd(
                key,
                k =>
                {
                    var cfg = configurations[key];
                    if (cfg == null)
                    {
                        throw new System.InvalidOperationException(
                            $"Configuration with {key} does not exists!"
                        );
                    }
                    var sessionFactory = cfg.BuildSessionFactory();
                    return sessionFactory;
                }
            );
        }

        public void SetSessionFactory(ISessionFactory sessionFactory)
        {
            sessionFactories[DefaultSessionFactoryKey] = sessionFactory;
        }

        public void SetSessionFactory(string key, ISessionFactory sessionFactory)
        {
            sessionFactories[key] = sessionFactory;
        }

        public global::NHibernate.Cfg.Configuration GetConfiguration()
        {
            return configurations[DefaultConfigurationKey];
        }

        public global::NHibernate.Cfg.Configuration GetConfiguration(string key)
        {
            return configurations[key];
        }

        public void SetConfiguration(global::NHibernate.Cfg.Configuration configuration)
        {
            configurations[DefaultConfigurationKey] = configuration;
        }

        public void SetConfiguration(string key, global::NHibernate.Cfg.Configuration configuration)
        {
            configurations[key] = configuration;
        }
    }

}
