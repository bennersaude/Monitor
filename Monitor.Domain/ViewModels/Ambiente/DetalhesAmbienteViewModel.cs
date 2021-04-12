using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Ambiente
{
    public class DetalhesAmbienteViewModel : EntidadeAuditavelViewModel
    {
        public string Nome { get; set; }
        [Display(Name = "Ativo")]
        public bool MonitoramentoAtivo { get; set; } = true;        
        [Display(Name = "Intervalo em Segundos para checar os WebServices")]
        public int IntervaloSegundosWebServiceChecks { get; set; }
        [Display(Name = "Timeout em milissegundos para a checagem dos WebServices")]
        public int TimeoutMilissegundosWebServiceChecks { get; set; }
        [Display(Name = "Quantidade Checagens para o Status do WebService")]
        public int QuantidadeChecagensConsiderarStatusWebService { get; set; }
        [Display(Name = "Quantidade de Dias para Excluir Dados antigos")]
        public int QuantidadeDiasExcluirDados { get; set; }
    }
}