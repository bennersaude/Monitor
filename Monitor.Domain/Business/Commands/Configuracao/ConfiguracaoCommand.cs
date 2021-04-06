namespace Monitor.Domain.Business.Commands.Configuracao
{
    public abstract class ConfiguracaoCommand : Command
    {
        public int IntervaloSegundosRecarregarAmbientes { get; protected set; }
    }
}