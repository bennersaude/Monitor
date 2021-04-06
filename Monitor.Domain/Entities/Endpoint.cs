using System;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;
using Monitor.Data.Infra.Attributes;

namespace Monitor.Domain.Entities
{
    public class Endpoint: EntidadeAuditavel
    {
        public virtual long HandleSistema { get; set; }
        public virtual Sistema Sistema { get; set; }

        [StringLength(70)]
        [RemoveExtraSpaces]
        public virtual string Nome { get; set; }

        [StringLength(1000)]
        [RemoveExtraSpaces]
        public virtual string Url { get; set; }

        public virtual void CopiarConfiguracoes(Endpoint ambienteOrigem)
        {
            this.Nome = ambienteOrigem.Nome;
            this.Url = ambienteOrigem.Url;
            this.HandleSistema = ambienteOrigem.HandleSistema;
        }

    }
}