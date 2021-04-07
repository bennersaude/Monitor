using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;
using Monitor.Data.Infra.Attributes;

namespace Monitor.Domain.Entities
{
    public class Sistema : EntidadeAuditavel
    {
        public virtual long HandleAmbiente { get; set; }
        public virtual Ambiente Ambiente { get; set; }

        [StringLength(70)]
        [RemoveExtraSpaces]
        public virtual string Nome { get; set; }

        [StringLength(70)]
        [RemoveExtraSpaces]
        public virtual string Cliente { get; set; }

        [StringLength(14)]
        public virtual string Cnpj { get; set; }

        public virtual bool MonitoramentoAtivo { get; set; }

        [StringLength(1000)]
        public virtual string UrlConsultaProcessos { get; set; }

        public virtual ICollection<Endpoint> Endpoints { get; set; }

        public override string ToString()
        {
            return $"{Nome} - {Cliente} - CNPJ: {Cnpj}";
        }

        public virtual void CopiarConfiguracoes(Sistema sistemaOrigem)
        {
            this.Nome = sistemaOrigem.Nome;
            this.MonitoramentoAtivo = sistemaOrigem.MonitoramentoAtivo;
            this.HandleAmbiente = sistemaOrigem.HandleAmbiente;
            this.Cliente = sistemaOrigem.Cliente;
            this.Cnpj = sistemaOrigem.Cnpj;
            this.UrlConsultaProcessos = sistemaOrigem.UrlConsultaProcessos;
        }
    }
}