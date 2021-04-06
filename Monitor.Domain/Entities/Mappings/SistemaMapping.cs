using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class SistemaMapping: ClassMap<Sistema>
    {
        public SistemaMapping()
        {
            Table(nameof(Sistema));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(Sistema)}");
            
            Map(x => x.CreateDateTime);
            Map(x => x.UpdateDateTime).Nullable();
            Map(x => x.Nome);
            Map(x => x.Cliente);
            Map(x => x.Cnpj);
            Map(x => x.MonitoramentoAtivo);
            HasMany(x => x.Endpoints).Cascade.AllDeleteOrphan();
            References(x => x.Ambiente, "HandleAmbiente");
        }
    }
}