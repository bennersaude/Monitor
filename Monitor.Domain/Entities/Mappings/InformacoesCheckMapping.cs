using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class InformacoesCheckMapping: ClassMap<InformacoesCheck>
    {
        public InformacoesCheckMapping()
        {
            Table(nameof(InformacoesCheck));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(InformacoesCheck)}");
            
            Map(x => x.HandleAmbiente).Index("IDX_InfCheck_Ambiente");
            Map(x => x.HandleSistema).Index("IDX_InfCheck_Sistema");
            Map(x => x.Url);
            Map(x => x.NomeSistema).Nullable().Index("IDX_InfCheck_NomeSistema");
            Map(x => x.Cnpj).Nullable().Index("IDX_InfCheck_Cnpj");
            Map(x => x.DataHoraConsulta).Nullable().Index("IDX_InfCheck_DtCons");
            Map(x => x.BServerHost).Nullable();
            Map(x => x.BServerSistema).Nullable();
            Map(x => x.CustomSystem).Nullable();
            Map(x => x.EncryptVDb).Nullable();
            Map(x => x.LastOficial).Nullable();
            Map(x => x.NomeDoSistema).Nullable();
            Map(x => x.UltimaAlteracao).Nullable();
            Map(x => x.UltimaAlteracaoEncrypt).Nullable();
            Map(x => x.UltimaCorrecao).Nullable();
            Map(x => x.UltimaCorrecaoEncrypt).Nullable();
            Map(x => x.UltimaEspecifica).Nullable();
            Map(x => x.UltimaEspecificaEncrypt).Nullable();
            Map(x => x.UltimaParalela).Nullable();
            Map(x => x.UltimaParalelaEncrypt).Nullable();
            Map(x => x.VersaoDb).Nullable();
            Map(x => x.VersaoDoSistema).Nullable();
            Map(x => x.VerticalSystem).Nullable();
            Map(x => x.Mensagem).Nullable();
            Map(x => x.DataHoraRequisicao).Nullable();
            Map(x => x.DataHoraResposta).Nullable();
            Map(x => x.DuracaoMilisegundosRequisicao).Nullable();
            Map(x => x.Sucesso).Nullable();
       
        }
    }
}

