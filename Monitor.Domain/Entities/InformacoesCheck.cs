using System;
using System.ComponentModel.DataAnnotations;
using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class InformacoesCheck: Entidade
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
        [StringLength(100)]
        public virtual string BServerHost { get; set; }
        [StringLength(100)]
        public virtual string BServerSistema { get; set; }
        [StringLength(100)]
        public virtual string CustomSystem { get; set; }
        [StringLength(100)]
        public virtual string EncryptVDb { get; set; }
        [StringLength(100)]
        public virtual string LastOficial { get; set; }
        [StringLength(100)]
        public virtual string NomeDoSistema { get; set; }
        [StringLength(100)]
        public virtual string UltimaAlteracao { get; set; }
        [StringLength(100)]
        public virtual string UltimaAlteracaoEncrypt { get; set; }
        [StringLength(100)]
        public virtual string UltimaCorrecao { get; set; }
        [StringLength(100)]
        public virtual string UltimaCorrecaoEncrypt { get; set; }
        [StringLength(100)]
        public virtual string UltimaEspecifica { get; set; }
        [StringLength(100)]
        public virtual string UltimaEspecificaEncrypt { get; set; }
        [StringLength(100)]
        public virtual string UltimaParalela { get; set; }
        [StringLength(100)]
        public virtual string UltimaParalelaEncrypt { get; set; }
        [StringLength(100)]
        public virtual string VersaoDb { get; set; }
        [StringLength(100)]
        public virtual string VersaoDoSistema { get; set; }
        [StringLength(100)]
        public virtual string VerticalSystem { get; set; }
        [StringLength(1000)]
        public virtual string Mensagem { get; set; }
        public virtual DateTime? DataHoraRequisicao { get; set; }
        public virtual DateTime? DataHoraResposta { get; set; }
        public virtual long? DuracaoMilisegundosRequisicao { get; set; }
        public virtual bool Sucesso { get; set; }
    }
}