using Monitor.Domain.Business.Validations;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Ambiente
{
    public class EditarAmbienteCommand : AmbienteCommand
    {
        public EditarAmbienteCommand(long handle, 
            string nome, 
            bool monitoramentoAtivo,
            int intervaloSegundosWebServiceChecks,
            int timeoutMilissegundosWebServiceChecks,
            int quantidadeChecagensConsiderarStatusWebService,
            int quantidadeDiasExcluirDados)
        {
            this.Handle = handle;
            this.Nome = nome;
            this.MonitoramentoAtivo = monitoramentoAtivo;
            this.IntervaloSegundosWebServiceChecks = intervaloSegundosWebServiceChecks;
            this.TimeoutMilissegundosWebServiceChecks = timeoutMilissegundosWebServiceChecks;
            this.QuantidadeChecagensConsiderarStatusWebService = quantidadeChecagensConsiderarStatusWebService;
            this.QuantidadeDiasExcluirDados = quantidadeDiasExcluirDados;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new EditarAmbienteValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}