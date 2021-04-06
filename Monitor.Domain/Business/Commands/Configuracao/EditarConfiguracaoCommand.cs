using Monitor.Domain.Business.Validations;
using Monitor.Domain.Business.Validations.Configuracao;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Configuracao
{
    public class EditarConfiguracaoCommand: ConfiguracaoCommand
    {
        public EditarConfiguracaoCommand(int intervaloSegundosRecarregarAmbientes)
        {
            this.IntervaloSegundosRecarregarAmbientes = intervaloSegundosRecarregarAmbientes;
        }

        public override bool IsValid(ISession session = null)
        {
            ValidationResult = new EditarConfiguracaoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}