using Monitor.Domain.Business.Validations;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Ambiente
{
    public class IncluirAmbienteCommand : AmbienteCommand
    {
        public IncluirAmbienteCommand(string nome,
            bool monitoramentoAtivo,
            int intervaloSegundosWebServiceChecks,
            int timeoutMilissegundosWebServiceChecks,
            int quantidadeChecagensConsiderarStatusWebService,
            int quantidadeDiasExcluirDados)
        {
            this.Nome = nome;
            this.MonitoramentoAtivo = monitoramentoAtivo;
            this.IntervaloSegundosWebServiceChecks = intervaloSegundosWebServiceChecks;
            this.TimeoutMilissegundosWebServiceChecks = timeoutMilissegundosWebServiceChecks;
            this.QuantidadeChecagensConsiderarStatusWebService = quantidadeChecagensConsiderarStatusWebService;
            this.QuantidadeDiasExcluirDados = quantidadeDiasExcluirDados;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new IncluirAmbienteValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}