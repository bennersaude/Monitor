using System.Threading.Tasks;
using FluentValidation;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Business.Validations;
using Monitor.Domain.Business.Validations.Endpoint;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Endpoint
{
    public class EditarEndpointCommand: EndpointCommand
    {
        public EditarEndpointCommand(long handle, 
            string nome,
            string url,
            long handleSistema)
        {
            this.Handle = handle;
            this.Nome = nome;
            this.Url = url;
            this.HandleSistema = handleSistema;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new EditarEndpointValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}