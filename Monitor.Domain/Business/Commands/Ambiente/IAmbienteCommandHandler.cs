using System.Threading.Tasks;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;

namespace Monitor.Domain.Business.Commands.Ambiente
{
    public interface IAmbienteCommandHandler
    {
        Task<Entities.Ambiente> IncluirAsync(IncluirAmbienteCommand command);
        Task<Entities.Ambiente> EditarAsync(EditarAmbienteCommand command);
        Task ExcluirAsync(ExcluirAmbienteCommand command);
    }
}