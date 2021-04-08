using System.Threading.Tasks;

namespace Monitor.Domain.Business.Jobs.Informacoes
{
    public interface IConsultaInformacoesSistema
    {
         Task ConsultarInformacoesAsync(Entities.Sistema sistema, IInformacoesMonitor processosMonitor);
    }
}