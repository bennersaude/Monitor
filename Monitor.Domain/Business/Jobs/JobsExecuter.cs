using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using AutoMapper;
using log4net;
using Monitor.Data;
using Monitor.Domain.Business.Jobs.WebService;
using Monitor.Domain.Entities;
using NHibernate;
using Monitor.Domain.Business.Jobs.Processos;
using Monitor.Domain.Business.Jobs.Informacoes;

namespace Monitor.Domain.Business.Jobs
{
    public class JobsExecuter : IJobsExecuter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JobsExecuter));
        private const int MILLISECONDS_IN_SECOND = 1000;
        private const int MILLISECONDS_RESTART_DELAY = 1000;
        private const int QTD_DIAS_INTERVALO_EXCLUSAO = 1;
        private readonly ISessionProvider sessionProvider;
        private readonly IMapper mapper;
        //private readonly IOperadorasLoader operadorasLoader;
        private readonly Dictionary<Ambiente, CancellationTokenSource> jobsCancellationTokens = new Dictionary<Ambiente, CancellationTokenSource>();
        private readonly Dictionary<Ambiente, IEnumerable<IMonitorJob>> jobs = new Dictionary<Ambiente, IEnumerable<IMonitorJob>>();
        private readonly Dictionary<long, Ambiente> ambientesDict = new Dictionary<long, Ambiente>();

        public JobsExecuter(ISessionProvider sessionProvider,
            IMapper mapper)
            //IOperadorasLoader operadorasLoader)
        {
            this.sessionProvider = sessionProvider;
            this.mapper = mapper;
            //this.operadorasLoader = operadorasLoader;
        }

        public void Execute()
        {
            while (true)
            {
                logger.Info($"Atualizando configurações e informações dos ambientes");
                IEnumerable<Ambiente> ambientes;
                Configuracao configuracao;
                using (var session = sessionProvider.OpenSession())
                {
                    session.BeginTransaction();
                    configuracao = session.Get<Configuracao>(Configuracao.HANDLE_FIXO);
                    ambientes = session.Query<Ambiente>().ToArray();
                    if (!ambientes.Any())
                    {
                        logger.Info("Não existem ambientes cadastrados. Nada será monitorado.");
                    }
                    //operadorasLoader.SincronizarOperadoras(ambientes, session);
                    //ForcarCarregamentoSistemas(ambientes);
                    ExcluirDadosAntigos(ambientes, session);
                    session.GetCurrentTransaction().Commit();
                }
                AdicionarJobsParaNovosAmbientes(ambientes);
                EncerrarJobsParaAmbientesExcluidos(ambientes);
                AtualizarConfiguracoesAmbientes(ambientes);
                Thread.Sleep(configuracao.IntervaloSegundosRecarregarAmbientes * MILLISECONDS_IN_SECOND);
            }
        }

        private void ExcluirDadosAntigos(IEnumerable<Ambiente> ambientes, ISession session)
        {
            foreach (var ambiente in ambientes)
            {
                if ((DateTime.UtcNow - (ambiente.UltimaExclusaoDados ?? DateTime.UtcNow.AddDays(-QTD_DIAS_INTERVALO_EXCLUSAO * 2))).TotalDays >= QTD_DIAS_INTERVALO_EXCLUSAO)
                {
                    logger.Info($"Excluindo dados antigos de monitoramento para ambiente {ambiente.Nome}");
                    var dataCorte = DateTime.UtcNow.AddDays(-ambiente.QuantidadeDiasExcluirDados);
                    WebServiceMonitorJob.ExcluirDadosAntigos(ambiente.Handle, dataCorte, session);
                    ProcessosJob.ExcluirDadosAntigos(ambiente.Handle, dataCorte, session);
                    InformacoesJob.ExcluirDadosAntigos(ambiente.Handle, dataCorte, session);
                    ambiente.UltimaExclusaoDados = DateTime.UtcNow;
                }
            }
        }

        private static void ForcarCarregamentoSistemas(IEnumerable<Ambiente> ambientes)
        {
            /*foreach (var ambiente in ambientes)
            {
                ambiente.Sistemas.ToArray();
            }*/
            ambientes.ToArray();
        }

        private void AtualizarConfiguracoesAmbientes(IEnumerable<Ambiente> ambientes)
        {
            //foreach (var ambiente in ambientes.Where(x => x.DbConnectionException == null))
            foreach (var ambiente in ambientes)
            {
                //if (ambientesDict[ambiente.Handle].DbConnectionString == ambiente.DbConnectionString)
                if (ambientesDict[ambiente.Handle].Nome == ambiente.Nome)
                {
                    ambientesDict[ambiente.Handle].CopiarConfiguracoes(ambiente);
                }
                else
                {
                    Task.Run(() =>
                    {
                        EncerrarJobsParaAmbiente(ambiente);
                        Thread.Sleep(MILLISECONDS_RESTART_DELAY);
                        IniciarJobsParaAmbiente(ambiente);
                    });
                }
            }
        }

        private void EncerrarJobsParaAmbientesExcluidos(IEnumerable<Ambiente> ambientes)
        {
            foreach (var ambiente in jobs.Keys)
            {
                //if (!ambientes.Where(x => x.DbConnectionException == null).Any(x => x.Equals(ambiente)))
                if (!ambientes.Any(x => x.Equals(ambiente)))
                {
                    EncerrarJobsParaAmbiente(ambiente);
                }
            }
        }

        private void EncerrarJobsParaAmbiente(Ambiente ambiente)
        {
            jobsCancellationTokens[ambiente].Cancel();
            logger.Info($"Encerrados jobs de monitoramento para ambiente {ambiente.Nome}");
            jobsCancellationTokens.Remove(ambiente);
            jobs.Remove(ambiente);
            ambientesDict.Remove(ambiente.Handle);
        }

        private void AdicionarJobsParaNovosAmbientes(IEnumerable<Ambiente> ambientes)
        {
            //foreach (var ambiente in ambientes.Where(x => x.DbConnectionException == null))
            foreach (var ambiente in ambientes)
            {
                if (!jobs.ContainsKey(ambiente))
                {
                    IniciarJobsParaAmbiente(ambiente);
                }
            }
        }

        private void IniciarJobsParaAmbiente(Ambiente ambiente)
        {
            logger.Info($"Iniciando jobs de monitoramento para ambiente {ambiente.Nome}");
            var monitors = new IMonitorJob[]
            {
                new WebServiceMonitorJob(sessionProvider,
                    ambiente,
                    new WebServiceStatusMonitor(sessionProvider),
                    new ConectividadeServico(new Ping(new WebRequestCreate()))),
                new ProcessosJob(sessionProvider,
                    ambiente,
                    new ProcessosMonitor(sessionProvider)),
                new InformacoesJob(sessionProvider,
                    ambiente,
                    new InformacoesMonitor(sessionProvider))
            };
            var cancellationTokenSource = new CancellationTokenSource();
            jobs.Add(ambiente, monitors);
            jobsCancellationTokens.Add(ambiente, cancellationTokenSource);
            ambientesDict.Add(ambiente.Handle, ambiente);
            foreach (var monitor in monitors)
            {
                Task.Run(() => monitor.StartMonitoring(cancellationTokenSource.Token));
            }
        }
    }
}