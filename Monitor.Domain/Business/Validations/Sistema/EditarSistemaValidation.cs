using System;
using System.Linq;
using Monitor.Domain.Business.Commands.Ambiente;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Validations.Ambiente;
using Monitor.Domain.Business.Commands.Sistema;

namespace Monitor.Domain.Business.Validations.Sistema
{
    public class EditarSistemaValidation: SistemaValidation<EditarSistemaCommand>
    {
        public EditarSistemaValidation(ISession session):base(session)
        {
            ValidarNome();            
            ValidarCliente();
            ValidarCNPJ();
            ValidarSeRegistroExiste();
            ValidarExistenciaAmbiente();
            ValidarDuplicidadeDeNome();            
        }
    }
}