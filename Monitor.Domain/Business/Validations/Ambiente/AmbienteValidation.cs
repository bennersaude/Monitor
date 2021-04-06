using System.Linq;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Business.Commands.Ambiente;
using FluentValidation;
using NHibernate;

namespace Monitor.Domain.Business.Validations.Ambiente
{
    public abstract class AmbienteValidation<T> : AbstractValidator<T> where T : AmbienteCommand
    {
        private readonly ISession session;

        protected AmbienteValidation(ISession session)
        {
            this.session = session ?? throw new System.ArgumentNullException(nameof(session));
        }

        protected void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Obrigatório informar o nome do ambiente")
                .Length(1, 70).WithMessage("O nome deve conter no máximo 70 caracteres");
        }

        protected void ValidateHandle()
        {
            RuleFor(c => c.Handle)
                .GreaterThan(0).WithMessage("Handle deve ser maior que zero");
        }

        protected void ValidarDuplicidadeDeNome()
        {
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (session.Query<Entities.Ambiente>().Any(x => x.Nome.ToLower() == c.Nome.ToLower() && x.Handle != c.Handle))
                    {
                        context.AddFailure("Já existe outro ambiente com o nome fornecido");
                    }
                });
        }

        protected void ValidarSeRegistroExiste()
        {
            RuleFor(c => c.Handle)
                .Custom((handle, context) =>
                {
                    if (!session.Query<Entities.Ambiente>().Any(x => x.Handle == handle))
                    {
                        context.AddFailure("Registro não localizado");
                    }
                });
        }

        protected void ValidateIntervaloSegundosWebServiceChecks()
        {
            RuleFor(c => c.IntervaloSegundosWebServiceChecks)
                .GreaterThan(0).WithMessage("O intervalo em segundos para checagem dos webservices deve ser maior que zero");
        }

        protected void ValidateTimeoutMilissegundosWebServiceChecks()
        {
            RuleFor(c => c.TimeoutMilissegundosWebServiceChecks)
                .GreaterThanOrEqualTo(500).WithMessage("O timeout em milissegundos dos webservices deve ser maior ou igual a 500");
        }

        protected void ValidateQuantidadeChecagensStatus()
        {
            RuleFor(c => c.QuantidadeChecagensConsiderarStatusWebService)
                .GreaterThan(0).WithMessage("A quantidade de checagens a considerar no status dos webservices deve ser maior que zero");
        }

        protected void ValidateQuantidadeDiasExcluirDados()
        {
            RuleFor(c => c.QuantidadeDiasExcluirDados)
                .GreaterThan(0).WithMessage("A quantidade de dias para excluir os dados deve ser maior que zero");
        }

    }
}