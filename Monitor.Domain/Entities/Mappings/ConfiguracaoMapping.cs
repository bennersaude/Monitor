using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class ConfiguracaoMapping : ClassMap<Configuracao>
    {
        public ConfiguracaoMapping()
        {
            Table(nameof(Configuracao));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Assigned();
            
            Map(x => x.CreateDateTime);
            Map(x => x.UpdateDateTime).Nullable();
            Map(x => x.IntervaloSegundosRecarregarAmbientes);
        }
    }
}