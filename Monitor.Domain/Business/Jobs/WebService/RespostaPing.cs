using System;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public class RespostaPing
    {
        private const string SUCESSO = "Data/Hora: {0} Url: {1} Teste de Conectividade: Sucesso! Código Http: 200";
        private const string FALHA = "Data/Hora: {0} Url: {1} Teste de Conectividade: Falha! Mensagem: {2}";
        private const string FALHAHTTP = "Data/Hora: {0} Url: {1} Teste de Conectividade: Falha! Código Http: {2} Mensagem: {3}";
        public int? HttpStatus { get; set; }
        public string Mensagem { get; set; }
        public bool ServicoAtivo { get; set; }
        public string Url { get; set; }
        public string DataHora { get; set; }

        public string MensagemFormatada
        {
            get
            {
                string mensagemFormatada;
                DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (ServicoAtivo)
                {
                    mensagemFormatada = string.Format(SUCESSO, DataHora, Url);
                }
                else
                {
                    mensagemFormatada = HttpStatus == null ? 
                        string.Format(FALHA, DataHora, Url, Mensagem) : 
                        string.Format(FALHAHTTP, DataHora, Url, HttpStatus, Mensagem);
                }

                return mensagemFormatada;
            }
        }
    }
}