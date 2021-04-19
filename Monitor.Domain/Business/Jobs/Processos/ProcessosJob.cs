using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Monitor.Data;
using Monitor.Data.Infra.Counter;
using Monitor.Domain.Entities;
using NHibernate;
using NHibernate.Linq;
using Polly;
using Polly.Timeout;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public class ProcessosJob : IMonitorJob
    {
        private const string TIMEOUT = "timeout";
        private const int MILLISECONDS_IN_SECOND = 1000;
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProcessosJob));
        private readonly ISessionProvider monitorSessionProvider;
        private readonly Ambiente ambiente;
        private readonly IProcessosMonitor processosMonitor;

        public ProcessosJob(ISessionProvider monitorSessionProvider,
            Ambiente ambiente, ProcessosMonitor processosMonitor)
        {
            this.monitorSessionProvider = monitorSessionProvider;
            this.ambiente = ambiente;
            this.processosMonitor = processosMonitor;
        }

        public void StartMonitoring(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    logger.Info("Monitoramento cancelado");
                    ct.ThrowIfCancellationRequested();
                }

                if (ambiente.MonitoramentoAtivo)
                {
                    ConsultarProcessosSistemas();
                }

                Thread.Sleep(ambiente.IntervaloSegundosWebServiceChecks * MILLISECONDS_IN_SECOND);
            }
        }

        private void ConsultarProcessosSistemas()
        {
            IEnumerable<Sistema> sistemas;
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                sistemas = monitorSession.Query<Sistema>().Where(c => c.Ambiente.Handle == ambiente.Handle 
                  && c.UrlConsultaProcessos != null && c.UrlConsultaProcessos != string.Empty);

                foreach (var sistema in sistemas)
                {
                    try
                    {  
                        var nomeAmbiente = sistema.Ambiente.Nome; 
                        ConsultarProcessosSistema(sistema); 
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"*** Falha ao consultar processos para o ambiente {ambiente.Nome} e sistema {sistema.Nome}: {ex}");
                    }
                }
            }
        }

        private void ConsultarProcessosSistema(Entities.Sistema sistema)
        {
            logger.Info($"[{ambiente.Nome}] Consultando processos do sistema {sistema.Nome}");
            var consultaProcessosSistema = new ConsultaProcessosSistema(monitorSessionProvider);
            Task.Run(() => consultaProcessosSistema.ConsultarProcessosAsync(sistema, processosMonitor));
        }

        public static void ExcluirDadosAntigos(long handleAmbiente, DateTime dataCorte, ISession session)
        {
            session.Query<ProcessosCheck>()
                .Where(c => c.HandleAmbiente == handleAmbiente && c.DataHoraConsulta <= dataCorte)
                .Delete();
        }

    }
}