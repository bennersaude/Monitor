using System.Threading.Tasks;
using FluentValidation;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Business.Validations;
using Monitor.Domain.Business.Validations.Endpoint;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Endpoint
{
    public class ExcluirEndpointCommand: EndpointCommand
    {
        public ExcluirEndpointCommand(long handle)
        {
            this.Handle = handle;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new ExcluirEndpointValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}