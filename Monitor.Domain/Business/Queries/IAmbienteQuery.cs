using System.Collections.Generic;
using System.Threading.Tasks;
using Monitor.Domain.ViewModels;
using Monitor.Domain.ViewModels.Ambiente;

namespace Monitor.Domain.Business.Queries
{
    public interface IAmbienteQuery
    {
        Task<IEnumerable<DetalhesAmbienteViewModel>> ListarAmbientesAsync();
        Task<DetalhesAmbienteViewModel> DetalhesAmbienteAsync(long handle);
    }
}