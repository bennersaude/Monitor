using System;
using System.Linq;
using Monitor.Domain.Business.Commands.Ambiente;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Validations.Ambiente;

namespace Monitor.Domain.Business.Validations
{
    public class ExcluirAmbienteValidation : AmbienteValidation<ExcluirAmbienteCommand>
    {
        public ExcluirAmbienteValidation(ISession session) : base(session)
        {
            ValidarSeRegistroExiste();
        }
    }
}