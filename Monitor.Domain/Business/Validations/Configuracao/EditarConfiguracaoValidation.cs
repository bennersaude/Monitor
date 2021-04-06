using Monitor.Domain.Business.Commands.Configuracao;

namespace Monitor.Domain.Business.Validations.Configuracao
{
    public class EditarConfiguracaoValidation: ConfiguracaoValidation<EditarConfiguracaoCommand>
    {
        public EditarConfiguracaoValidation()
        {
            ValidarIntervaloSegundosRecarregarAmbientes();
        }
    }
}