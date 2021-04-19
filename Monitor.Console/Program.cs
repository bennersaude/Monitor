using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitor.Data;
using Monitor.Data.Types;
using AutoMapper;
using Monitor.Data.Infra.Helpers;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.Entities;
using NHibernate;
using Monitor.Domain.Business.Jobs;

namespace Monitor.Console
{
    class Program
    {
        public static IConfiguration Configuration;
        private static ISessionProvider sessionProvider;
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            StartUp();
            sessionProvider.UpdateSchema();
            SetFirstTimeConfiguration(sessionProvider);
            using (var scope = serviceProvider.CreateScope())
            {
                var jobsExecuter = scope.ServiceProvider.GetService<IJobsExecuter>();
                jobsExecuter.Execute();
            }
        }

        private static void SetFirstTimeConfiguration(ISessionProvider sessionProvider)
        {
            var query = serviceProvider.GetService<IConfiguracaoQuery>();
            var configuracao = Task.Run(() => query.ObterConfiguracaoAsync()).Result;
            if (configuracao == null)
            {
                using (var session = sessionProvider.OpenSession())
                {
                    session.BeginTransaction();
                    session.Save(new Configuracao());
                    session.GetCurrentTransaction().Commit();
                }
            }
        }

        private static void StartUp()
        {
            System.Console.OutputEncoding = Encoding.UTF8;

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            System.Console.WriteLine("Environment: {0}", environment);

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {

                builder
                    .AddJsonFile(
                        Path.Combine(Directory.GetCurrentDirectory(), string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar),
                            $"appsettings.{environment}.json"),
                        optional: true
                    );
            }

            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddLogging(x => x.ClearProviders().AddLog4Net());

            var connectionString = Environment.GetEnvironmentVariable("BENNERMONITOR_CONNECTIONSTRING")
                ?? Configuration.GetConnectionString("BennerMonitor");
            System.Console.WriteLine("ConnectionString: {0}", connectionString);
            if (String.IsNullOrEmpty(connectionString))
                throw new ConfigurationErrorsException($"Não encontrada string de conexão 'BennerMonitor'!");
                
            sessionProvider = new SessionProvider(TipoAplicacao.StandAlone,
                connectionString,
                assembliesWithMappingsPatterns: new[] { "Monitor.Data", "Monitor.Domain" },
                false);
            var path = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                    "hibernate.config"
                    );
            sessionProvider.Configure(services, path);
            services.AddSingleton<ISessionProvider>(sessionProvider);
            Domain.DependencyInjectionHelper.RegisterTypes(services);
            var automapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(DomainHelper.GetAssembly("Monitor.Domain"));
            });

            services.AddSingleton(automapperConfig.CreateMapper());
            serviceProvider = services.BuildServiceProvider();
        }
    }
}
