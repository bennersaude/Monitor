using System.ComponentModel.DataAnnotations;
using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Sistema
{
    public class EditarSistemaViewModel: EntidadeViewModel
    {
        public long HandleAmbiente { get; set; }
        public string Nome { get; set; }
        public string Cliente { get; set; }
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }
        [Display(Name = "Ativo")]
        public bool MonitoramentoAtivo { get; set; } = true;
        [Display(Name = "URL para Consulta dos Processos")]
        public string UrlConsultaProcessos { get; set; }
        [Display(Name = "URL para Consulta das Informações")]
        public string UrlConsultaInformacoes { get; set; }
    }
}