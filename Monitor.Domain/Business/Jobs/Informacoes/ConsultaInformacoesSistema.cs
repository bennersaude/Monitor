using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Monitor.Data;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Informacoes;

namespace Monitor.Domain.Business.Jobs.Informacoes
{
    public class ConsultaInformacoesSistema : IConsultaInformacoesSistema
    {
        private readonly HttpClient client = new HttpClient();
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConsultaInformacoesSistema));
        private readonly ISessionProvider monitorSessionProvider;

        public ConsultaInformacoesSistema(ISessionProvider monitorSessionProvider)
        {
            this.monitorSessionProvider = monitorSessionProvider;
        }
        public async Task ConsultarInformacoesAsync(Sistema sistema, IInformacoesMonitor informacoesMonitor)
        {
            string endpointCompleto=string.Empty;
            var data = DateTime.Now;
            var informacoesSistema = new InformacoesViewModel{Sucesso = false};
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(sistema.Ambiente.TimeoutMilissegundosWebServiceChecks);
                endpointCompleto = sistema.UrlConsultaInformacoes.EndsWith('/') ? 
                  sistema.UrlConsultaInformacoes.Substring(0,sistema.UrlConsultaInformacoes.Length-1) : sistema.UrlConsultaInformacoes;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var streamTask = client.GetStreamAsync(endpointCompleto);
                informacoesSistema = await JsonSerializer.DeserializeAsync<InformacoesViewModel>(await streamTask);
                informacoesSistema.Sucesso = true;
            }
            catch (System.Exception ex)
            {
                logger.Info($"Problema ao consultar Informações do endpoint: {endpointCompleto}. Erro: {ex.ToString()}.");
                informacoesSistema.Mensagem = ex.ToString().Substring(0, Math.Min(ex.ToString().Length, 1000));
            }

            informacoesMonitor.RegistrarInformacoes(sistema, informacoesSistema, endpointCompleto, data); 
        }
    }
}