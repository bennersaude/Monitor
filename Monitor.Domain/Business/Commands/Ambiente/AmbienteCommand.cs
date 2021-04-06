namespace Monitor.Domain.Business.Commands.Ambiente
{
    public abstract class AmbienteCommand : Command
    {
        public long Handle { get; protected set; }
        public string Nome { get; protected set; }
        public bool MonitoramentoAtivo { get; protected set; }
        public int IntervaloSegundosWebServiceChecks { get; protected set; }
        public int TimeoutMilissegundosWebServiceChecks { get; protected set; }
        public int QuantidadeChecagensConsiderarStatusWebService { get; protected set; }
        public int QuantidadeDiasExcluirDados { get; protected set; }
    }
}