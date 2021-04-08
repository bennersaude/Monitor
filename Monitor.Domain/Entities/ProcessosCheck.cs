using System;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class ProcessosCheck: Entidade
    {
        public virtual long HandleAmbiente { get; set; }
        public virtual long HandleSistema { get; set; }
        [StringLength(1000)]
        public virtual string Url { get; set; }
        [StringLength(80)]
        public virtual string NomeSistema { get; set; }
        [StringLength(14)]
        public virtual string Cnpj { get; set; }
        public virtual DateTime DataHoraConsulta { get; set; }
        public virtual long? ProcessosPendentes { get; set; }
        public virtual long? ProcessosExecutando { get; set; }
        public virtual long? TotalFinalizadosSucesso { get; set; }
        public virtual long? TotalFinalizadosErro { get; set; }
        public virtual long? FinalizadosSucesso { get; set; }
        public virtual long? FinalizadosErro { get; set; }
        [StringLength(1000)]
        public virtual string Mensagem { get; set; }
        public virtual DateTime? DataHoraRequisicao { get; set; }
        public virtual DateTime? DataHoraResposta { get; set; }
        public virtual long? DuracaoMilisegundosRequisicao { get; set; }
        public virtual bool Sucesso { get; set; }
    }
}