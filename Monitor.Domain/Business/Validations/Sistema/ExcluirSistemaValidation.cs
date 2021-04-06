using System;
using System.Linq;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Commands.Sistema;

namespace Monitor.Domain.Business.Validations.Sistema
{
    public class ExcluirSistemaValidation: SistemaValidation<ExcluirSistemaCommand>
    {
        public ExcluirSistemaValidation(ISession session) : base(session)
        {
            ValidarSeRegistroExiste();
        }
    }
}