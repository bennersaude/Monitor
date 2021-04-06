namespace Monitor.Domain.Business.Commands.Endpoint
{
    public abstract class EndpointCommand : Command
    {
        public long Handle { get; protected set; }
        public long HandleSistema { get; protected set; }
        public string Nome { get; protected set; }
        public string Url { get; protected set; }
    }
}