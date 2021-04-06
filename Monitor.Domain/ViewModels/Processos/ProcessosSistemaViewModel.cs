using System;

namespace Monitor.Domain.ViewModels.Processos
{
    public class ProcessosSistemaViewModel
    {
        public string Sistema { get; set; }
        public DateTime Data { get; set; }
        public long ProcessosPendentes { get; set; }
        public long ProcessosExecutando { get; set; }
        public long FinalizadosSucesso { get; set; }
        public long FinalizadosErro { get; set; }
        public string Mensagem { get; set; }
        
    }
}