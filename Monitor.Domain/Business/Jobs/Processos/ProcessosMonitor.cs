using System;
using System.Linq;
using Monitor.Data;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Processos;
using NHibernate;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public class ProcessosMonitor : IProcessosMonitor
    {
        private readonly ISessionProvider monitorSessionProvider;
        public ProcessosMonitor(ISessionProvider monitorSessionProvider)
        {
            this.monitorSessionProvider = monitorSessionProvider;
        }
        public void RegistrarProcessos(Sistema sistema, ProcessosSistemaViewModel processos, string Endpoint, DateTime data)
        {
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                long? totalAnteriorProcessosFinalizadosSucesso = 0;
                long? totalAnteriorProcessosFinalizadosErro = 0;
                var registroAnterior = monitorSession.Query<ProcessosCheck>()
                    .Where(x => x.HandleSistema == sistema.Handle && x.DataHoraConsulta >= data.Date && x.DataHoraConsulta < data)
                    .OrderByDescending(x => x.DataHoraConsulta).Take(1).ToList();
                
                if ((registroAnterior != null) && (registroAnterior.Any()))
                {
                    totalAnteriorProcessosFinalizadosSucesso = registroAnterior.First().TotalFinalizadosSucesso;
                    totalAnteriorProcessosFinalizadosErro = registroAnterior.First().TotalFinalizadosErro;
                }
                
                var dados = new ProcessosCheck()
                {
                    HandleAmbiente = sistema.Ambiente.Handle,
                    HandleSistema = sistema.Handle,
                    Url = Endpoint,
                    NomeSistema = processos.Sistema,
                    Cnpj = sistema.Cnpj,
                    DataHoraConsulta = data,
                    ProcessosPendentes = processos.ProcessosPendentes,
                    ProcessosExecutando = processos.ProcessosExecutando,
                    TotalFinalizadosSucesso = processos.FinalizadosSucesso,
                    TotalFinalizadosErro = processos.FinalizadosErro,
                    FinalizadosErro = processos.FinalizadosErro - totalAnteriorProcessosFinalizadosErro,
                    FinalizadosSucesso = processos.FinalizadosSucesso - totalAnteriorProcessosFinalizadosSucesso,
                    Mensagem = processos.Mensagem,
                    DataHoraRequisicao = data,
                    DataHoraResposta = DateTime.Now,
                    DuracaoMilisegundosRequisicao = Convert.ToInt64((DateTime.Now - data).TotalMilliseconds)
                };
                
                monitorSession.BeginTransaction();
                monitorSession.Save(dados);
                monitorSession.GetCurrentTransaction().Commit();
            }
        }
    }
}