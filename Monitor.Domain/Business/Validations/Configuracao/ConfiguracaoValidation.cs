using Monitor.Domain.Business.Commands.Configuracao;
using FluentValidation;

namespace Monitor.Domain.Business.Validations.Configuracao
{
    public abstract class ConfiguracaoValidation<T> : AbstractValidator<T> where T : ConfiguracaoCommand
    {
        protected void ValidarIntervaloSegundosRecarregarAmbientes()
        {
            RuleFor(c => c.IntervaloSegundosRecarregarAmbientes)
                .GreaterThan(30).WithMessage("Defina um intervalo em segundos para recarregar configurações e ambientes igual ou superior a 30 segundos.");
        }
    }
}