using Monitor.Domain.Entities;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public interface IWebServiceStatusMonitor
    {
        void RegistrarStatus(Sistema sistema);
    }
}