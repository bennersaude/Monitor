namespace Monitor.Domain.ViewModels.Sistema
{
    public class IncluirSistemaViewModel
    {
        public long HandleAmbiente { get; set; }
        public string Nome { get; set; }
        public string Cliente { get; set; }
        public string Cnpj { get; set; }
        public bool MonitoramentoAtivo { get; set; } = true;
        public string UrlConsultaProcessos { get; set; }
    }
}