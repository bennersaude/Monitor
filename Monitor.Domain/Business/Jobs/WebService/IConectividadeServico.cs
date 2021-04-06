namespace Monitor.Domain.Business.Jobs.WebService
{
    public interface IConectividadeServico
    {
        RespostaPing TestaConectividade(string enderecoServico);
    }
}