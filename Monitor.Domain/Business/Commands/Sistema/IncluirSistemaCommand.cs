using System.Threading.Tasks;
using FluentValidation;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Business.Validations;
using Monitor.Domain.Business.Validations.Sistema;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Sistema
{
    public class IncluirSistemaCommand: SistemaCommand
    {
        public IncluirSistemaCommand(string nome,
            bool monitoramentoAtivo,
            long handleAmbiente,
            string cliente,
            string cnpj)
        {
            this.Nome = nome;
            this.MonitoramentoAtivo = monitoramentoAtivo;
            this.HandleAmbiente = handleAmbiente;
            this.Cliente = cliente;
            this.Cnpj = cnpj;
        }

        public override bool IsValid(ISession session)
        {
            ValidationResult = new IncluirSistemaValidation(session).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}