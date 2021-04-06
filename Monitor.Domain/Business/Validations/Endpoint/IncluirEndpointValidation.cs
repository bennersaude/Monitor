using System;
using System.Linq;
using Monitor.Domain.Business.Commands.Ambiente;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Validations.Ambiente;
using Monitor.Domain.Business.Commands.Endpoint;

namespace Monitor.Domain.Business.Validations.Endpoint
{
    public class IncluirEndpointValidation: EndpointValidation<IncluirEndpointCommand>
    {
        public IncluirEndpointValidation(ISession session):base(session)
        {
            ValidarNome();            
            ValidarUrl();
            ValidarExistenciaSistema();
            ValidarDuplicidadeDeNome();            
        }
    }
}