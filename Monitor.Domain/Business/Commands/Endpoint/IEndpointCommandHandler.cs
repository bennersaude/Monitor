using System.Threading.Tasks;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;

namespace Monitor.Domain.Business.Commands.Endpoint
{
    public interface IEndpointCommandHandler
    {
        Task<Entities.Endpoint> IncluirAsync(IncluirEndpointCommand command);
        Task<Entities.Endpoint> EditarAsync(EditarEndpointCommand command);
        Task ExcluirAsync(ExcluirEndpointCommand command);
    }
}