using Monitor.ComponentModel;

namespace Monitor.Domain.Entities
{
    public class Configuracao : EntidadeAuditavel
    {
        public const long HANDLE_FIXO = 1;

        public Configuracao()
        {
            IntervaloSegundosRecarregarAmbientes = 60;
            Handle = HANDLE_FIXO;
        }

        public virtual int IntervaloSegundosRecarregarAmbientes { get; set; }
    }
}