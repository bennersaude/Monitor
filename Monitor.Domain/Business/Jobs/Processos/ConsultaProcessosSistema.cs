using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Monitor.Data;
using Monitor.Domain.ViewModels.Processos;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public class ConsultaProcessosSistema : IConsultaProcessosSistema
    {
        private readonly HttpClient client = new HttpClient();
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProcessosJob));
        private readonly ISessionProvider monitorSessionProvider;

        public ConsultaProcessosSistema(ISessionProvider monitorSessionProvider)
        {
            this.monitorSessionProvider = monitorSessionProvider;
        }
        public async Task ConsultarProcessosAsync(Entities.Sistema sistema, ProcessosMonitor processosMonitor)
        {
            string endpointCompleto=string.Empty;
            var data = DateTime.Now;
            var processosSistema = new ProcessosSistemaViewModel();
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(sistema.Ambiente.TimeoutMilissegundosWebServiceChecks);
                var dataString = string.Format("/{0}-{1}-{2}", data.Year.ToString("00"), data.Month.ToString("00"), data.Day.ToString("00"));
                endpointCompleto = sistema.UrlConsultaProcessos.EndsWith('/') ? 
                  sistema.UrlConsultaProcessos.Substring(0,sistema.UrlConsultaProcessos.Length-1) : sistema.UrlConsultaProcessos;
                endpointCompleto = String.Concat(endpointCompleto,dataString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var streamTask = client.GetStreamAsync(endpointCompleto);
                processosSistema = await JsonSerializer.DeserializeAsync<ProcessosSistemaViewModel>(await streamTask);
            }
            catch (System.Exception ex)
            {
                logger.Info($"Problema ao consultar Processos do endpoint: {endpointCompleto}. Erro: {ex.ToString()}.");
                processosSistema.Mensagem = ex.ToString().Substring(0, Math.Min(ex.ToString().Length, 1000));
            }

            processosMonitor.RegistrarProcessos(sistema, processosSistema, endpointCompleto, data); 
            
        }
    }
}