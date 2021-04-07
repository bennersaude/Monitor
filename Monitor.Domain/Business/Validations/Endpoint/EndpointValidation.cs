using System.Linq;
using Monitor.Domain.Business.Commands;
using FluentValidation;
using NHibernate;
using Monitor.Domain.Business.Commands.Endpoint;

namespace Monitor.Domain.Business.Validations.Endpoint
{
    public abstract class EndpointValidation<T> : AbstractValidator<T> where T : EndpointCommand
    {
        private readonly ISession session;

        protected EndpointValidation(ISession session)
        {
            this.session = session ?? throw new System.ArgumentNullException(nameof(session));
        }

        protected void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Obrigatório informar o nome do ambiente")
                .Length(1, 70).WithMessage("O nome deve conter no máximo 70 caracteres");
        }

        protected void ValidarUrl()
        {
            RuleFor(c => c.Url)
                .NotEmpty().WithMessage("Obrigatório informar a URL do Endpoint")
                .Length(1, 1000).WithMessage("A Url deve conter no máximo 1000 caracteres");
        }

        protected void ValidateHandle()
        {
            RuleFor(c => c.Handle)
                .GreaterThan(0).WithMessage("Handle deve ser maior que zero");
        }

        protected void ValidarExistenciaSistema()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (!session.Query<Entities.Sistema>().Any(x => x.Handle == c.HandleSistema))
                    {
                        context.AddFailure("Não existe sistema com o handle informado");
                    }
                });
        }

        protected void ValidarDuplicidadeDeNome()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (session.Query<Entities.Endpoint>().Any(x => x.Nome.ToLower() == c.Nome.ToLower() && x.Handle != c.Handle 
                      && x.Sistema.Handle == c.HandleSistema))
                    {
                        context.AddFailure("Já existe outro Endpoint para o mesmo sistema com o nome fornecido");
                    }
                });
        }

        protected void ValidarDuplicidadeDeUrl()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (session.Query<Entities.Endpoint>().Any(x => x.Url.ToLower() == c.Url.ToLower() && x.Handle != c.Handle 
                      && x.Sistema.Handle == c.HandleSistema))
                    {
                        context.AddFailure("Já existe outro Endpoint para o mesmo ambiente com a url fornecida");
                    }
                });
        }

        protected void ValidarSeRegistroExiste()
        {
            RuleFor(c => c.Handle)
                .Custom((handle, context) =>
                {
                    if (!session.Query<Entities.Endpoint>().Any(x => x.Handle == handle))
                    {
                        context.AddFailure("Registro não localizado");
                    }
                });
        }

    }
}