using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Sistema
{
    public class DetalhesSistemaViewModel: EntidadeAuditavelViewModel
    {
        public long HandleAmbiente { get; set; }
        public string Nome { get; set; }
        public string Cliente { get; set; }
        public string Cnpj { get; set; }
        public bool MonitoramentoAtivo { get; set; }
        public string UrlConsultaProcessos { get; set; }
    }
}