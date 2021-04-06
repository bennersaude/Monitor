using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Configuracao
{
    public class DetalhesConfiguracaoViewModel : EntidadeAuditavelViewModel
    {
        public int IntervaloSegundosRecarregarAmbientes { get; set; }
    }
}