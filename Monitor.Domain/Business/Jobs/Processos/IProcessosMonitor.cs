using System;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Processos;

namespace Monitor.Domain.Business.Jobs.Processos
{
    public interface IProcessosMonitor
    {
         void RegistrarProcessos(Sistema sistema, ProcessosSistemaViewModel processos, string Endpoint, DateTime data);
    }
}