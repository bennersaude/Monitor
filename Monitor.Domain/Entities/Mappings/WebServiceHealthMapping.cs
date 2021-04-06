using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class WebServiceHealthMapping: ClassMap<WebServiceHealth>
    {
        public WebServiceHealthMapping()
        {
            Table(nameof(WebServiceHealth));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(WebServiceHealth)}");        
                
            Map(x => x.HandleAmbiente).Index("IDX_WSStat_Ambiente");            
            Map(x => x.NomeServicoIntegracao).Nullable().Index("IDX_WSStat_NomeServico");
            Map(x => x.Url).Index("IDX_WSStat_Url");
            Map(x => x.HandleSistema).Nullable().Index("IDX_WSStat_Sistema");
            Map(x => x.Cnpj).Nullable().Index("IDX_WSStat_Cnpj");
            Map(x => x.DataHoraInicioStatus).Index("IDX_WSStat_Inicio");
            Map(x => x.DataHoraFimStatus).Index("IDX_WSStat_Fim");
            Map(x => x.DataHoraStatus).Index("IDX_WSStat_Datahora");
            Map(x => x.QuantidadeChecagensConsiderada, "QtdChecagens");
            Map(x => x.QuantidadeWebServices);
            Map(x => x.TempoMedioRespostaMilisegundos, "TempoMediaResposta").Nullable();
            Map(x => x.Status).Index("IDX_WSStat_Status");
        }
    }
}