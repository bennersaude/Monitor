using System.Collections.Generic;

namespace Monitor.Domain.ViewModels.Ambiente
{
    public class IncluirAmbienteViewModel
    {
        public string Nome { get; set; }
        public bool MonitoramentoAtivo { get; set; } = true;
        public int IntervaloSegundosWebServiceChecks { get; set; }
        public int TimeoutMilissegundosWebServiceChecks { get; set; }
        public int QuantidadeChecagensConsiderarStatusWebService { get; set; }
        public int QuantidadeDiasExcluirDados { get; set; }
    }
}