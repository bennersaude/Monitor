using System;
using System.Linq;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Commands.Endpoint;

namespace Monitor.Domain.Business.Validations.Endpoint
{
    public class ExcluirEndpointValidation: EndpointValidation<ExcluirEndpointCommand>
    {
        public ExcluirEndpointValidation(ISession session) : base(session)
        {
            ValidarSeRegistroExiste();
        }
    }
}