using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Monitor.ComponentModel;
using Monitor.Data.Config;
using Monitor.Data.Types;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Persister.Entity;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using Monitor.Data.Mapping;
using Monitor.Data.Infra;
using Monitor.Data.Sql;
using Monitor.Data.NHibernate.Conventions;

namespace Monitor.Data
{
    public class SessionProvider: ISessionProvider
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(SessionProvider));
        private ISessionFactory sessionFactory;
        public IConfigurationProvider ConfigProvider { get; private set; }
        public string ConnectionString { get; private set; }
        public static string ChangeToSchema { get; private set; }
        public bool SessionFactoryCacheEnabled => ConfigurationManager.AppSettings["nHibernate.SessionFactory.Monitor.Enabled"] != null &&
                                                         ConfigurationManager.AppSettings["nHibernate.SessionFactory.Monitor.Enabled"].ToLower() == "true";

        private readonly object lockObject = new object();
        private readonly TipoAplicacao tipoAplicacao;
        private readonly IEnumerable<string> assembliesWithMappingsPatterns;
        private readonly bool readonlySchema;
        private const string NHIBERNATE_DEFAULT_CONNECTION_NAME = "nHibernateConnectionName";

        public ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                    throw new InvalidOperationException("SessionFactory ainda não inicializada. Utilizar o método BuildSessionFactory primeiro");

                return sessionFactory;
            }
        }

        public DataBaseType DataBaseType => DataBaseType.SQLServer; //new DataBaseHelper().DiscoverDataBaseType();

        public IEnumerable<string> TableNames;
        private global::NHibernate.Cfg.Configuration hibernateConfiguration;
        public SessionProvider(
            TipoAplicacao tipoAplicacao,
            string connectionString,
            IEnumerable<string> assembliesWithMappingsPatterns,
            bool readonlySchema)
        {
            this.tipoAplicacao = tipoAplicacao;
            this.assembliesWithMappingsPatterns = assembliesWithMappingsPatterns;
            this.readonlySchema = readonlySchema;
            SetNHibernateConnectionString(connectionString);
        }

        public ISession OpenSession()
        {
            return BuildSessionFactory().OpenSession();
        }

        public IStatelessSession OpenStatelessSession()
        {
            return BuildSessionFactory().OpenStatelessSession();
        }

        public ISession GetCurrentSession()
        {
            return BuildSessionFactory().GetCurrentSession();
        }

        public string DefaultSchema
        {
            get
            {
                if (TableNames == null || !TableNames.Any())
                    return string.Empty;
                var firstTable = TableNames.First();
                if (firstTable.Contains("."))
                    return firstTable.Split('.').First();
                return string.Empty;
            }
        }

        public void Configure(IServiceCollection services,
            string configPath)
        {
            Monitor.Data.Config.ConfigurationProvider.ConfigFilePath = configPath;
            AddConfigurationProvider(services);
            services.AddSingleton(provider =>
            {
                var cfgProvider = provider.GetService<IConfigurationProvider>();
                return cfgProvider.GetConfiguration();
            });
        }

        private void AddConfigurationProvider(
            IServiceCollection services
        )
        {
            if (ConfigProvider == null)
            {
                ConfigProvider = new Monitor.Data.Config.ConfigurationProvider();
                services.AddSingleton(ConfigProvider);
            }
        }

        public void Configure(IConfigurationProvider configurationProvider)
        {
            ConfigProvider = configurationProvider;
        }

        public ISessionFactory BuildSessionFactory()
        {
            lock (lockObject)
            {
                // Se a sessão já existir, retorná-la
                if (sessionFactory != null)
                    return sessionFactory;

                var fluentConfigure = Fluently.Configure();
                fluentConfigure = fluentConfigure.Mappings(m => m.FluentMappings.AddFromAssemblyList(GetAssembliesWithMappings().ToList())
                    .Conventions.AddFromAssemblyOf<PropertiesSqlTypeConvention>().Conventions.Add(AutoImport.Never()));

                switch (tipoAplicacao)
                {
                    case TipoAplicacao.Portal:
                        fluentConfigure = fluentConfigure.CurrentSessionContext<global::NHibernate.Context.WebSessionContext>();
                        break;
                    case TipoAplicacao.Servico:
                        throw new InvalidOperationException("tipo de aplicação não suportado");
                    case TipoAplicacao.StandAlone:
                        fluentConfigure = fluentConfigure.CurrentSessionContext<global::NHibernate.Context.ThreadStaticSessionContext>();
                        break;
                    default:
                        throw new InvalidOperationException("tipo de aplicação desconhecida");
                }
                hibernateConfiguration = fluentConfigure.BuildConfiguration();
                hibernateConfiguration.Configure(Monitor.Data.Config.ConfigurationProvider.ConfigFilePath);
                ConfigProvider.SetConfiguration(hibernateConfiguration);

                hibernateConfiguration.Properties["connection.connection_string"] = ConnectionString;
                hibernateConfiguration.Proxy(x => x.ProxyFactoryFactory<global::NHibernate.Bytecode.StaticProxyFactoryFactory>());
                hibernateConfiguration.SessionFactory().GenerateStatistics();
                sessionFactory = hibernateConfiguration.BuildSessionFactory();
                for(int i = 0; i < sessionFactory.Statistics.EntityNames.Count(); i++)
                    logger.DebugFormat("ENTITYNAME {0}", sessionFactory.Statistics.EntityNames[i]);
                return sessionFactory;
            }
        }

        private IEnumerable<Assembly> GetAssembliesWithMappings()
        {
            var assemblies = new List<Assembly>();
            foreach (var pattern in assembliesWithMappingsPatterns)
            {
                DomainLoader.ForceAssemblyLoading(pattern, TipoAplicacao.StandAlone);
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.Contains(pattern));
                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }
            }
            logger.Info($"Carregando mapeamentos dos assemblies:");
            foreach (var assembly in assemblies)
            {
                logger.Info($"- {assembly.FullName}");
            }

            return assemblies;
        }

        private string SetNHibernateConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ConfigurationErrorsException("Nenhuma string de conexão configurada para acesso ao banco de dados!");
            var split = connectionString.Split(';');
            ConnectionString = string.Empty;
            var termosPadrao = new string[]
            {
                "Password",
                "User ID",
                "Data Source",
                "Server",
                "Database",
                "Trusted_Connection",
                "Initial Catalog",
                "Min Pool Size",
                "Max Pool Size",
                "Pooling",
                "PoolBlockingPeriod",
                "Enlist",
                "Connection Lifetime"
            };

            foreach (var s in split)
            {
                if (termosPadrao.Any(t => s.StartsWith(t, StringComparison.InvariantCultureIgnoreCase)))
                    ConnectionString += s + ";";
                if (s.StartsWith("ChangeToSchema", StringComparison.InvariantCultureIgnoreCase) && s.Contains('='))
                    ChangeToSchema = s.Split('=').Last();
            }

            return ConnectionString;
        }

        public string GetClassMappingTable(Entidade entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            string table = null;
            var metadata = SessionFactory.GetClassMetadata(entity.GetType());
            if (metadata is AbstractEntityPersister)
                table = (metadata as AbstractEntityPersister).TableName;
            return table;
        }

        public void UpdateSchema()
        {
            if (readonlySchema)
            {
                throw new InvalidOperationException("O esquema é somente leitura! Atualização de esquema não permitida!");
            }
            var mappingsCommands = new List<string>();
            BuildSessionFactory();
            var updater = new SchemaUpdate(hibernateConfiguration);
            logger.Info("Atualizando esquema do banco de dados a partir dos mapeamentos...");
            updater.Execute(c => mappingsCommands.Add(c), doUpdate: false);
            ColumnDefaults.FixFKColumnsType(mappingsCommands, Enumerable.Empty<string>());
            var sqlExecuter = new DirectDbSqlExecuter();
            using (var session = OpenSession())
            {
                foreach (var sql in mappingsCommands)
                {
                    sqlExecuter.Execute(sql, session);
                }
            }
            if (updater.Exceptions?.Any() == true)
            {
                var sb = new StringBuilder("Ocorreram os seguintes erros ao atualizar o esquema do banco de dados:");
                foreach (var error in updater.Exceptions)
                {
                    sb.AppendLine(error.ToString());
                }
                logger.Error(sb.ToString());
                throw new AggregateException("Falha ao atualizar esquema do banco de dados!", updater.Exceptions);
            }
        }
    }
}