namespace Monitor.Domain.Business.Commands.Sistema
{
    public abstract class SistemaCommand : Command
    {
        public long Handle { get; protected set; }
        public long HandleAmbiente { get; protected set; }
        public string Nome { get; protected set; }
        public string Cliente { get; protected set; }
        public string Cnpj { get; protected set; }
        public bool MonitoramentoAtivo { get; protected set; }
        public string UrlConsultaProcessos { get; protected set; }
    }
}