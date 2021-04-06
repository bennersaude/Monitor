using System.Threading.Tasks;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;

namespace Monitor.Domain.Business.Commands.Sistema
{
    public interface ISistemaCommandHandler
    {
        Task<Entities.Sistema> IncluirAsync(IncluirSistemaCommand command);
        Task<Entities.Sistema> EditarAsync(EditarSistemaCommand command);
        Task ExcluirAsync(ExcluirSistemaCommand command);
    }
}