using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class AmbienteMapping : ClassMap<Ambiente>
    {
        public AmbienteMapping()
        {
            Table(nameof(Ambiente));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(Ambiente)}");
            
            Map(x => x.CreateDateTime);
            Map(x => x.UpdateDateTime).Nullable();
            Map(x => x.Nome).Index("IDX_Ambiente_Nome");
            HasMany(x => x.Sistemas).Cascade.AllDeleteOrphan();
            Map(x => x.MonitoramentoAtivo);
            Map(x => x.IntervaloSegundosWebServiceChecks, "WSSEGUNDOSESPERA").Default("60");
            Map(x => x.TimeoutMilissegundosWebServiceChecks, "WSTIMEOUTMS").Default("5000");
            Map(x => x.QuantidadeChecagensConsiderarStatusWebService, "QTDCHECAGENSSTATUSWS").Default("3");;
            Map(x => x.QuantidadeDiasExcluirDados, "QTDDIASEXCLUIRDADOS").Default("90");
            //Map(x => x.AtivarParalelismoWebServicesOperadora, "WSATIVARPARALELISMO");            
            Map(x => x.UltimaExclusaoDados);
            //Map(x => x.DiferencaHorasUtc).Default("-3");
        }
    }
}