using FluentNHibernate.Mapping;

namespace Monitor.Domain.Entities.Mappings
{
    public class ProcessosCheckMapping: ClassMap<ProcessosCheck>
    {
        public ProcessosCheckMapping()
        {
            Table(nameof(ProcessosCheck));
            Id(x => x.Handle).Not.Nullable().GeneratedBy.Sequence($"SEQ_{nameof(ProcessosCheck)}");
            
            Map(x => x.HandleAmbiente).Index("IDX_ProcCheck_Ambiente");
            Map(x => x.HandleSistema).Index("IDX_ProcCheck_Sistema");
            Map(x => x.Url);
            Map(x => x.NomeSistema).Nullable().Index("IDX_ProcCheck_NomeSistema");
            Map(x => x.Cnpj).Nullable().Index("IDX_ProcCheck_Cnpj");
            Map(x => x.DataHoraConsulta).Nullable().Index("IDX_ProcCheck_DtCons");
            Map(x => x.ProcessosPendentes).Nullable();
            Map(x => x.ProcessosExecutando).Nullable();
            Map(x => x.TotalFinalizadosSucesso).Nullable();
            Map(x => x.TotalFinalizadosErro).Nullable();
            Map(x => x.FinalizadosSucesso).Nullable();
            Map(x => x.FinalizadosErro).Nullable();
            Map(x => x.Mensagem).Nullable();
            Map(x => x.DataHoraRequisicao).Nullable();
            Map(x => x.DataHoraResposta).Nullable();
            Map(x => x.DuracaoMilisegundosRequisicao).Nullable();;
        }
    }
}

