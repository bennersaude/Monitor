using System.Collections.Generic;
using System.Threading.Tasks;
using Monitor.Domain.ViewModels;
using Monitor.Domain.ViewModels.Endpoint;

namespace Monitor.Domain.Business.Queries
{
    public interface IEndpointQuery
    {
        Task<IEnumerable<DetalhesEndpointViewModel>> ListarEndpointsAsync();
    }
}