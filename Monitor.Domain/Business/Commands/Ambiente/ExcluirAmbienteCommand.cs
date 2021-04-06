using Monitor.Domain.Business.Validations;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Ambiente
{
    public class ExcluirAmbienteCommand : AmbienteCommand
    {
        public ExcluirAmbienteCommand(long handle)
        {
            this.Handle = handle;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new ExcluirAmbienteValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}