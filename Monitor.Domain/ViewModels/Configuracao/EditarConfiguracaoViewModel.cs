using System.ComponentModel.DataAnnotations;

namespace Monitor.Domain.ViewModels.Configuracao
{
    public class EditarConfiguracaoViewModel
    {
        [Display(Name = "Intervalo em segundos para recarregar os ambientes")]
        public int IntervaloSegundosRecarregarAmbientes { get; set; }
    }
}