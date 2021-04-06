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
        //private readonly ISessionProvider ambienteSessionProvider;
        private readonly Ambiente ambiente;
        //private readonly IConectividadeServico conectividadeServico;
        //private readonly IWebServiceStatusMonitor statusMonitor;

        public ProcessosJob(ISessionProvider monitorSessionProvider,
            Ambiente ambiente)
            //IWebServiceStatusMonitor statusMonitor,
            //IConectividadeServico conectividadeServico)
        {
            this.monitorSessionProvider = monitorSessionProvider;
            this.ambiente = ambiente;
            //this.conectividadeServico = conectividadeServico;
            //this.statusMonitor = statusMonitor;
            //this.ambienteSessionProvider = ImportedDomain.SessionProviderFactory.GetSessionProvider(
            //    ambiente.DbConnectionString, monitorSessionProvider.ConfigProvider);
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
            //IEnumerable<Endpoint> webservices;
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                sistemas = monitorSession.Query<Sistema>().Where(c => c.Ambiente.Handle == ambiente.Handle);

                foreach (var sistema in sistemas.Where(x => x.Nome == "QUALIDADEAGCORRENTE"))
                {
                    try
                    {  
                        var nomeAmbiente = sistema.Ambiente.Nome; 
                        ConsultarProcessosSistema(sistema); 
                        /*webservices = monitorSession.Query<Endpoint>().Where(c => c.Sistema.Handle == sistema.Handle);
                
                        var webservicesDict = webservices.ToDictionary(k => k, v => new Uri(v.Url));
                        
                        if (webservicesDict.Any())
                        {
                            TestarConectividadeSistema(sistema, webservicesDict);
                        }
                        else
                        {
                            logger.Info($"[{ambiente.Nome}] Nenhum webservice do sistema {sistema.Nome} está configurado. Testes ignorados.");
                        }*/
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
            //var checks = new BlockingCollection<WebServiceCheck>();
            /*var idBateriaTestes = GetNextValue("SEQ_WebServiceCheck");
            var dataHoraBateriaTestes = DateTime.UtcNow;
            foreach (var webservice in webservices)
            {
                TestarWebservice(sistema, webservice, checks, idBateriaTestes, dataHoraBateriaTestes);
            };*/

            var consultaProcessosSistema = new ConsultaProcessosSistema();
            Task.Run(() => consultaProcessosSistema.ConsultarProcessosAsync("http://mga-apl044/QualidadeAG_Corrente/api/z_processos/resumodiario/2021-04-06"));

            //SalvarDados(checks);
            //Task.Run(() => statusMonitor.RegistrarStatus(sistema)).ConfigureAwait(false);
        }

        /*private void TestarConectividadeSistema(Entities.Sistema sistema, Dictionary<Entities.Endpoint, Uri> webservices)
        {
            logger.Info($"[{ambiente.Nome}] Testando {webservices.Count()} webservices do sistema {sistema.Nome}");
            var checks = new BlockingCollection<WebServiceCheck>();
            var idBateriaTestes = GetNextValue("SEQ_WebServiceCheck");
            var dataHoraBateriaTestes = DateTime.UtcNow;
            foreach (var webservice in webservices)
            {
                TestarWebservice(sistema, webservice, checks, idBateriaTestes, dataHoraBateriaTestes);
            };
            SalvarDados(checks);
            Task.Run(() => statusMonitor.RegistrarStatus(sistema)).ConfigureAwait(false);
        }

        private void TestarWebservice(Entities.Sistema sistema, KeyValuePair<Entities.Endpoint, Uri> webservice, 
            BlockingCollection<WebServiceCheck> checks, long idBateriaTestes, DateTime dataHoraBateriaTestes)
        {
            var timer = new Stopwatch();
            Exception excecao = null;
            RespostaPing respostaPing = null;
            int? httpStatusError = null;
            DateTime? dataHoraRequisicao = null;
            
            timer.Start();
            try
            {
                dataHoraRequisicao = DateTime.UtcNow;
                var timeoutPolicy = Policy.Timeout(TimeSpan.FromMilliseconds(ambiente.TimeoutMilissegundosWebServiceChecks), TimeoutStrategy.Pessimistic);
                timeoutPolicy.Execute(() =>
                    respostaPing = conectividadeServico.TestaConectividade(webservice.Value.ToString()));
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                {
                    httpStatusError = (int?)((ex as WebException)?.Response as HttpWebResponse)?.StatusCode;
                }
                excecao = ex;
            }
            timer.Stop();


            var dados = new WebServiceCheck()
            {
                HandleAmbiente = ambiente.Handle,
                HandleSistema = sistema.Handle,
                HandleEndpoint = webservice.Key.Handle,
                Cnpj = sistema.Cnpj,
                BateriaTestes = idBateriaTestes,
                DataHoraBateriaTestes = dataHoraBateriaTestes,
                Url = webservice.Value.ToString(),
                NomeServicoIntegracao = webservice.Key.Nome.ToString(),
                DataHoraRequisicao = dataHoraRequisicao,
                DataHoraResposta = respostaPing != null ? DateTime.UtcNow : (DateTime?)null,
                DuracaoMilisegundosRequisicao = respostaPing != null ? timer.ElapsedMilliseconds : (long?)null,
                TimeoutMilisegundos = IsTimeoutException(excecao)
                    ? ambiente.TimeoutMilissegundosWebServiceChecks : (int?)null,
                HttpStatusResposta = respostaPing?.HttpStatus ?? httpStatusError,
                DataHoraExcecao = excecao != null ? DateTime.UtcNow : (DateTime?)null,
                DetalhesExcecao = excecao?.ToString().Length > 4000 ? excecao?.ToString().Substring(0,4000) : excecao?.ToString(),
                TipoExcecao = excecao?.GetType().FullName.Length > 250 ? excecao?.GetType().FullName.Substring(0, 250) : excecao?.GetType().FullName
            };
            checks.Add(dados);
        }

        private long GetNextValue(string sequenceName)
        {
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                var counterHelper = new CounterHelper(monitorSession);
                return counterHelper.GetNextValue(sequenceName);
            }
        }

        private void SalvarDados(BlockingCollection<WebServiceCheck> checks)
        {
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                monitorSession.BeginTransaction();
                foreach (var dados in checks)
                {
                    monitorSession.Save(dados);
                }
                monitorSession.GetCurrentTransaction().Commit();
            }
        }

        private static bool IsTimeoutException(Exception excecao)
        {
            return excecao?.GetType().FullName.ToLower().Contains(TIMEOUT) == true;
        }

        public static void ExcluirDadosAntigos(DateTime dataCorte, ISession session)
        {
            session.Query<WebServiceCheck>()
                .Where(c => c.DataHoraBateriaTestes <= dataCorte)
                .Delete();

            session.Query<WebServiceHealth>()
                .Where(c => c.DataHoraStatus <= dataCorte)
                .Delete();
        }*/
    }
}