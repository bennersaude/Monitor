using System.ComponentModel.DataAnnotations;
using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Configuracao
{
    public class DetalhesConfiguracaoViewModel : EntidadeAuditavelViewModel
    {
        [Display(Name = "Intervalo em segundos para recarregar os ambientes")]
        public int IntervaloSegundosRecarregarAmbientes { get; set; }
    }
}