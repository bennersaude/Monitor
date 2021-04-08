using System;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Informacoes;

namespace Monitor.Domain.Business.Jobs.Informacoes
{
    public interface IInformacoesMonitor
    {
         void RegistrarInformacoes(Sistema sistema, InformacoesViewModel informacoes, string Endpoint, DateTime data);
    }
}