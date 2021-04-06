using Monitor.Domain.ViewModels.Entidade;

namespace Monitor.Domain.ViewModels.Endpoint
{
    public class EditarEndpointViewModel: EntidadeViewModel
    {
        public long HandleSistema { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
    }
}