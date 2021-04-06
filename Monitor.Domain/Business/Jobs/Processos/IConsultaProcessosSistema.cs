using System.Threading.Tasks;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public interface IConsultaProcessosSistema
    {
        Task ConsultarProcessosAsync(string endpoint);
         
    }
}