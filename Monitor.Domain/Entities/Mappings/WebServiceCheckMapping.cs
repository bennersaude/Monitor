using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class WebServiceCheckMapping: ClassMap<WebServiceCheck>
    {
        public WebServiceCheckMapping()
        {
            Table(nameof(WebServiceCheck));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(WebServiceCheck)}");
            
            Map(x => x.HandleAmbiente).Index("IDX_WSCheck_Ambiente");
            Map(x => x.BateriaTestes).Index("IDX_WSCheck_Bateria");
            Map(x => x.DataHoraBateriaTestes).Index("IDX_WSCheck_DtBateria");
            Map(x => x.HandleEndpoint).Nullable().Index("IDX_WSCheck_Endpoint");
            Map(x => x.NomeServicoIntegracao).Nullable().Index("IDX_WSCheck_NomeServico");
            Map(x => x.Url).Index("IDX_WSCheck_Url");
            Map(x => x.HandleSistema).Nullable().Index("IDX_WSCheck_Sistema");
            Map(x => x.Cnpj).Nullable().Index("IDX_WSCheck_Cnpj");
            Map(x => x.DataHoraRequisicao).Nullable();
            Map(x => x.DuracaoMilisegundosRequisicao);
            Map(x => x.TimeoutMilisegundos);
            Map(x => x.DataHoraResposta).Nullable();
            Map(x => x.HttpStatusResposta).Nullable().Index("IDX_WSCheck_HttpStatus");
            Map(x => x.DataHoraExcecao).Nullable();
            Map(x => x.DetalhesExcecao).Nullable();
            Map(x => x.TipoExcecao).Nullable();
        }
    }
}

