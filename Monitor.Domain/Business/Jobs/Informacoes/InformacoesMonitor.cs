using System;
using System.Linq;
using Monitor.Data;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Informacoes;
using NHibernate;

namespace Monitor.Domain.Business.Jobs.Informacoes
{
    public class InformacoesMonitor : IInformacoesMonitor
    {
        private readonly ISessionProvider monitorSessionProvider;
        public InformacoesMonitor(ISessionProvider monitorSessionProvider)
        {
            this.monitorSessionProvider = monitorSessionProvider;
        }
        public void RegistrarInformacoes(Sistema sistema, InformacoesViewModel informacoes, string Endpoint, DateTime data)
        {
            using (var monitorSession = monitorSessionProvider.OpenSession())
            {
                var gravarRegistro = true;
                if (informacoes.Sucesso)
                {
                    var registroAnterior = monitorSession.Query<InformacoesCheck>()
                        .Where(x => x.HandleSistema == sistema.Handle && x.DataHoraConsulta < data && x.Sucesso == true)
                        .OrderByDescending(x => x.DataHoraConsulta).Take(1).ToList();
                    
                    if ((registroAnterior != null) && (registroAnterior.Any()) && 
                        !HouveAtualizacaoInformacoes(registroAnterior.First(), informacoes))
                    {
                        gravarRegistro = false;
                    }
                }
                
                if (!gravarRegistro)
                    return;

                var dados = new InformacoesCheck()
                {
                    HandleAmbiente = sistema.Ambiente.Handle,
                    HandleSistema = sistema.Handle,
                    Url = Endpoint,
                    NomeSistema = informacoes.Sistema,
                    Cnpj = sistema.Cnpj,
                    DataHoraConsulta = data,
                    BServerHost = informacoes.BServerHost,
                    BServerSistema = informacoes.BServerSistema,
                    CustomSystem = informacoes.CustomSystem,
                    EncryptVDb = informacoes.EncryptVDb,
                    LastOficial = informacoes.LastOficial,
                    NomeDoSistema = informacoes.NomeDoSistema,
                    UltimaAlteracao = informacoes.UltimaAlteracao,
                    UltimaAlteracaoEncrypt = informacoes.UltimaAlteracaoEncrypt,
                    UltimaCorrecao = informacoes.UltimaCorrecao,
                    UltimaCorrecaoEncrypt = informacoes.UltimaCorrecaoEncrypt,
                    UltimaEspecifica = informacoes.UltimaEspecifica,
                    UltimaEspecificaEncrypt = informacoes.UltimaEspecificaEncrypt,
                    UltimaParalela = informacoes.UltimaParalela,
                    UltimaParalelaEncrypt = informacoes.UltimaParalelaEncrypt,
                    VersaoDb = informacoes.VersaoDb,
                    VersaoDoSistema = informacoes.VersaoDoSistema,
                    VerticalSystem = informacoes.VerticalSystem,
                    Mensagem = informacoes.Mensagem,
                    DataHoraRequisicao = data,
                    DataHoraResposta = DateTime.Now,
                    DuracaoMilisegundosRequisicao = Convert.ToInt64((DateTime.Now - data).TotalMilliseconds),
                    Sucesso = informacoes.Sucesso
                };
                
                monitorSession.BeginTransaction();
                monitorSession.Save(dados);
                monitorSession.GetCurrentTransaction().Commit();
            }
        }

        public bool HouveAtualizacaoInformacoes(InformacoesCheck registroAnterior, InformacoesViewModel novasInformacoes)
        {
            if ((novasInformacoes.BServerHost != registroAnterior.BServerHost) ||
                (novasInformacoes.BServerSistema != registroAnterior.BServerSistema) ||
                (novasInformacoes.CustomSystem != registroAnterior.CustomSystem) ||
                (novasInformacoes.EncryptVDb != registroAnterior.EncryptVDb) ||
                (novasInformacoes.LastOficial != registroAnterior.LastOficial) ||
                (novasInformacoes.Sistema != registroAnterior.NomeSistema) ||
                (novasInformacoes.NomeDoSistema != registroAnterior.NomeDoSistema) ||
                (novasInformacoes.UltimaAlteracao != registroAnterior.UltimaAlteracao) ||
                (novasInformacoes.UltimaAlteracaoEncrypt != registroAnterior.UltimaAlteracaoEncrypt) ||
                (novasInformacoes.UltimaCorrecao != registroAnterior.UltimaCorrecao) ||
                (novasInformacoes.UltimaCorrecaoEncrypt != registroAnterior.UltimaCorrecaoEncrypt) ||
                (novasInformacoes.UltimaEspecifica != registroAnterior.UltimaEspecifica) ||
                (novasInformacoes.UltimaEspecificaEncrypt != registroAnterior.UltimaEspecificaEncrypt) ||
                (novasInformacoes.UltimaParalela != registroAnterior.UltimaParalela) ||
                (novasInformacoes.UltimaParalelaEncrypt != registroAnterior.UltimaParalelaEncrypt) ||
                (novasInformacoes.VersaoDb != registroAnterior.VersaoDb) ||
                (novasInformacoes.VersaoDoSistema != registroAnterior.VersaoDoSistema) ||
                (novasInformacoes.VerticalSystem != registroAnterior.VerticalSystem)
            )
                return true;

            return false;
        }
    }
}