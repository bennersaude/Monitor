using System;
using System.Linq;
using Monitor.Domain.Business.Commands.Ambiente;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Validations.Ambiente;

namespace Monitor.Domain.Business.Validations
{
    public class EditarAmbienteValidation : AmbienteValidation<EditarAmbienteCommand>
    {
        public EditarAmbienteValidation(ISession session) : base(session)
        {
            ValidarNome();
            ValidarSeRegistroExiste();
            ValidarDuplicidadeDeNome();
            ValidateIntervaloSegundosWebServiceChecks();
            ValidateTimeoutMilissegundosWebServiceChecks();
            ValidateQuantidadeChecagensStatus();
            ValidateQuantidadeDiasExcluirDados();
        }
    }
}