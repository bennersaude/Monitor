using System.Linq;
using Monitor.Domain.Business.Commands;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Commands.Sistema;

namespace Monitor.Domain.Business.Validations.Sistema
{
    public abstract class SistemaValidation<T> : AbstractValidator<T> where T : SistemaCommand
    {
        private readonly ISession session;

        protected SistemaValidation(ISession session)
        {
            this.session = session ?? throw new System.ArgumentNullException(nameof(session));
        }

        protected void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Obrigatório informar o nome do ambiente")
                .Length(1, 70).WithMessage("O nome deve conter no máximo 70 caracteres");
        }

        protected void ValidarCliente()
        {
            RuleFor(c => c.Cliente)
                .NotEmpty().WithMessage("Obrigatório informar o cliente do ambiente")
                .Length(1, 70).WithMessage("O nome do cliente deve conter no máximo 70 caracteres");
        }

        protected void ValidarCNPJ()
        {
            RuleFor(c => c.Cnpj)
                .NotEmpty().WithMessage("Obrigatório informar o CNPJ do cliente")
                .Length(1, 14).WithMessage("O CNPJ deve conter no máximo 14 caracteres");
        }

        protected void ValidateHandle()
        {
            RuleFor(c => c.Handle)
                .GreaterThan(0).WithMessage("Handle deve ser maior que zero");
        }

        protected void ValidarExistenciaAmbiente()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (!session.Query<Entities.Ambiente>().Any(x => x.Handle == c.HandleAmbiente))
                    {
                        context.AddFailure("Não existe ambiente com o handle informado");
                    }
                });
        }

        protected void ValidarDuplicidadeDeNome()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (session.Query<Entities.Sistema>().Any(x => x.Nome.ToLower() == c.Nome.ToLower() && x.Handle != c.Handle 
                      && x.Ambiente.Handle == c.HandleAmbiente))
                    {
                        context.AddFailure("Já existe outro sistema para o mesmo ambiente com o nome fornecido");
                    }
                });
        }

        protected void ValidarSeRegistroExiste()
        {
            RuleFor(c => c.Handle)
                .Custom((handle, context) =>
                {
                    if (!session.Query<Entities.Sistema>().Any(x => x.Handle == handle))
                    {
                        context.AddFailure("Registro não localizado");
                    }
                });
        }

    }
}