using System;
using System.Threading.Tasks;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public interface IConsultaProcessosSistema
    {
        Task ConsultarProcessosAsync(Entities.Sistema sistema, IProcessosMonitor processosMonitor);
         
    }
}