using System.Threading.Tasks;
using Monitor.Domain.ViewModels.Configuracao;

namespace Monitor.Domain.Business.Queries
{
    public interface IConfiguracaoQuery
    {
         Task<DetalhesConfiguracaoViewModel> ObterConfiguracaoAsync();
    }
}