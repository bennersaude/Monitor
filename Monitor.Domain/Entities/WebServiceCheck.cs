using System;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class WebServiceCheck: Entidade
    {
        public virtual long HandleAmbiente { get; set; }
        public virtual long HandleSistema { get; set; }
        public virtual long HandleEndpoint { get; set; }
        [StringLength(80)]
        public virtual string NomeServicoIntegracao { get; set; }
        [StringLength(1000)]
        public virtual string Url { get; set; }
        [StringLength(14)]
        public virtual string Cnpj { get; set; }
        public virtual DateTime DataHoraBateriaTestes { get; set; }
        public virtual DateTime? DataHoraRequisicao { get; set; }
        public virtual DateTime? DataHoraResposta { get; set; }
        public virtual long? DuracaoMilisegundosRequisicao { get; set; }
        public virtual long? TimeoutMilisegundos { get; set; }
        public virtual int? HttpStatusResposta { get; set; }
        public virtual DateTime? DataHoraExcecao { get; set; }
        [StringLength(4000)]
        public virtual string DetalhesExcecao { get; set; }
        [StringLength(250)]
        public virtual string TipoExcecao { get; set; }
        public virtual long BateriaTestes { get; set; }
    }
}