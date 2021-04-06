using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class Ambiente : EntidadeAuditavel
    {
        [Required]
        [StringLength(70)]
        public virtual string Nome { get; set; }

        public virtual bool MonitoramentoAtivo { get; set; } = true;

        public virtual int IntervaloSegundosWebServiceChecks { get; set; }

        public virtual int TimeoutMilissegundosWebServiceChecks { get; set; }

        public virtual int QuantidadeChecagensConsiderarStatusWebService { get; set; }

        public virtual int QuantidadeDiasExcluirDados { get; set; }

        public virtual DateTime? UltimaExclusaoDados { get; set; }


        public virtual ICollection<Sistema> Sistemas { get; set; }

        public virtual void CopiarConfiguracoes(Ambiente ambienteOrigem)
        {
            this.Nome = ambienteOrigem.Nome;
            this.MonitoramentoAtivo = ambienteOrigem.MonitoramentoAtivo;
            this.IntervaloSegundosWebServiceChecks = ambienteOrigem.IntervaloSegundosWebServiceChecks;
            this.QuantidadeChecagensConsiderarStatusWebService = ambienteOrigem.QuantidadeChecagensConsiderarStatusWebService;
            this.TimeoutMilissegundosWebServiceChecks = ambienteOrigem.TimeoutMilissegundosWebServiceChecks;
            this.QuantidadeDiasExcluirDados = ambienteOrigem.QuantidadeDiasExcluirDados;
        }
    }
}