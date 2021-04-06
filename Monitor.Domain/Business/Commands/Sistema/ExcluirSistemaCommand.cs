using System.Threading.Tasks;
using FluentValidation;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Business.Validations;
using Monitor.Domain.Business.Validations.Sistema;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Sistema
{
    public class ExcluirSistemaCommand: SistemaCommand
    {
        public ExcluirSistemaCommand(long handle)
        {
            this.Handle = handle;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new ExcluirSistemaValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}