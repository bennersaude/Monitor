using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Monitor.Domain.Entities;
using log4net;
using Monitor.Data;
using NHibernate;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public class WebServiceStatusMonitor : IWebServiceStatusMonitor
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WebServiceStatusMonitor));
        private readonly ISessionProvider monitorSessionProvider;

        public WebServiceStatusMonitor(ISessionProvider monitorSessionProvider)
        {
            this.monitorSessionProvider = monitorSessionProvider;
        }

        public void RegistrarStatus(Entities.Sistema sistema)
        {
            logger.Info($"[{sistema.Ambiente.Nome}] Registrando status dos webservices do sistema {sistema.Nome}");
            using (var session = monitorSessionProvider.OpenSession())
            {
                session.BeginTransaction();
                var bateriaTestesConsiderar = session.Query<WebServiceCheck>()
                    .Where(x => x.HandleSistema == sistema.Handle)
                    .Select(x => x.BateriaTestes).OrderByDescending(x => x).Distinct().Take(sistema.Ambiente.QuantidadeChecagensConsiderarStatusWebService).ToList();
                if (bateriaTestesConsiderar.Any())
                {
                    var dataHora = DateTime.UtcNow;
                    var checks = session.Query<WebServiceCheck>()
                        .Where(x => x.HandleSistema == sistema.Handle &&
                                    x.BateriaTestes >= bateriaTestesConsiderar.Last()).ToArray();
                    RegistrarPorWebService(checks, dataHora, sistema, session);
                    RegistrarPorSistema(checks, dataHora, sistema, session);
                }
                session.GetCurrentTransaction().Commit();
            }
        }

        private void RegistrarPorSistema(WebServiceCheck[] checks, DateTime dataHora, Sistema sistema, NHibernate.ISession session)
        {
            var status = new WebServiceHealth()
            {
                HandleAmbiente = sistema.Ambiente.Handle,
                HandleSistema = sistema.Handle,
                Cnpj = sistema.Cnpj,
                DataHoraInicioStatus = checks.Min(x => x.DataHoraBateriaTestes),
                DataHoraFimStatus = checks.Max(x => x.DataHoraBateriaTestes),
                DataHoraStatus = dataHora,
                QuantidadeChecagensConsiderada = checks.Count(),
                QuantidadeWebServices = checks.Select(x => x.Url).Distinct().Count(),
                TempoMedioRespostaMilisegundos = ObterTempoMedioResposta(checks),
                Status = CalcularStatus(checks)
            };
            session.Save(status);
        }

        private static long? ObterTempoMedioResposta(IEnumerable<WebServiceCheck> checks)
        {
            return checks.Any(x => x.DuracaoMilisegundosRequisicao.HasValue)
                                    ? Convert.ToInt64(checks.Where(x => x.DuracaoMilisegundosRequisicao.HasValue)
                                        .Average(x => x.DuracaoMilisegundosRequisicao.Value))
                                    : (long?)null;
        }

        private WebServiceStatus CalcularStatus(IEnumerable<WebServiceCheck> group)
        {
            var qtd = group.Count();
            var ok = group.Count(x => x.HttpStatusResposta == (int)HttpStatusCode.OK);
            var falha = group.Count(x => x.HttpStatusResposta != (int)HttpStatusCode.OK);
            
            if (qtd == ok)
            {
                return WebServiceStatus.Healthy;
            }
            if (qtd == falha)
            {
                return WebServiceStatus.Error;
            }
            return WebServiceStatus.Intermitent;
        }

        private void RegistrarPorWebService(WebServiceCheck[] checks, DateTime dataHora, Sistema sistema, NHibernate.ISession session)
        {
            var webServices = checks.GroupBy(x => x.HandleEndpoint);
            foreach (var group in webServices)
            {
                var status = new WebServiceHealth()
                {
                    HandleAmbiente = sistema.Ambiente.Handle,
                    HandleSistema = sistema.Handle,
                    NomeServicoIntegracao = group.FirstOrDefault(x => x.NomeServicoIntegracao != null)?.NomeServicoIntegracao,
                    Url = group.First().Url,
                    Cnpj = sistema.Cnpj,
                    DataHoraInicioStatus = group.Min(x => x.DataHoraBateriaTestes),
                    DataHoraFimStatus = group.Max(x => x.DataHoraBateriaTestes),
                    DataHoraStatus = dataHora,
                    QuantidadeChecagensConsiderada = group.Count(),
                    QuantidadeWebServices = 1,
                    TempoMedioRespostaMilisegundos = ObterTempoMedioResposta(group),
                    Status = CalcularStatus(group)
                };
                session.Save(status);
            }
        }
    }
}