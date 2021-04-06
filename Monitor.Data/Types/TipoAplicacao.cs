using System.ComponentModel;

namespace Monitor.Data.Types
{
    public enum TipoAplicacao
    {
        [Description("Portal")]
        Portal,
        [Description("Serviço")]
        Servico,
        [Description("Stand Alone")]
        StandAlone
    }
}