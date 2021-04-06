using System;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class WebServiceHealth: Entidade
    {
        public virtual long HandleAmbiente { get; set; }
        public virtual long HandleSistema { get; set; }
        [StringLength(80)]
        public virtual string NomeServicoIntegracao { get; set; }        
        [StringLength(1000)]
        public virtual string Url { get; set; }
        [StringLength(14)]
        public virtual string Cnpj { get; set; }
        public virtual DateTime DataHoraInicioStatus { get; set; }
        public virtual DateTime DataHoraFimStatus { get; set; }
        public virtual DateTime DataHoraStatus { get; set; }
        public virtual int QuantidadeChecagensConsiderada { get; set; }
        public virtual int QuantidadeWebServices { get; set; }
        public virtual long? TempoMedioRespostaMilisegundos { get; set; }
        public virtual WebServiceStatus Status { get; set; }
    }
}