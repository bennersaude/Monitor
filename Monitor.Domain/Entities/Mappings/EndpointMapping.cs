using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class EndpointMapping: ClassMap<Endpoint>
    {
        public EndpointMapping()
        {
            Table(nameof(Endpoint));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(Endpoint)}");
            
            Map(x => x.CreateDateTime);
            Map(x => x.UpdateDateTime).Nullable();
            Map(x => x.Nome);
            Map(x => x.Url);
            References(x => x.Sistema, "HandleSistema");
        }
    }
}