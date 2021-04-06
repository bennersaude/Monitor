using System.Threading.Tasks;

namespace Monitor.Domain.Business.Commands.Configuracao
{
    public interface IConfiguracaoCommandHandler
    {
         Task<Entities.Configuracao> EditarAsync(EditarConfiguracaoCommand command);
    }
}