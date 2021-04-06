using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Monitor.Domain.ViewModels.Processos;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public class ConsultaProcessosSistema : IConsultaProcessosSistema
    {
        private readonly HttpClient client = new HttpClient();
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProcessosJob));
        public async Task ConsultarProcessosAsync(string endpoint)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            /*client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");*/

            /*var stringTask = client.GetStringAsync(endpoint);
            var msg = await stringTask;
            logger.Info($"[Endpoint: {endpoint}] Resultado: {msg}");*/

            var streamTask = client.GetStreamAsync(endpoint);
            var processosSistema = await JsonSerializer.DeserializeAsync<ProcessosSistemaViewModel>(await streamTask);
            logger.Info($"[Endpoint: {endpoint}] Resultado: Sistema={processosSistema.Sistema} - FinalizadosSucesso={processosSistema.FinalizadosSucesso}");
        }
    }
}