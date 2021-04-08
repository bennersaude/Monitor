using System;

namespace Monitor.Domain.ViewModels.Informacoes
{
    public class InformacoesViewModel
    {
        public string Sistema { get; set; }
        public DateTime Data { get; set; }
        public string BServerHost { get; set; }
        public string BServerSistema { get; set; }
        public string CustomSystem { get; set; }
        public string EncryptVDb { get; set; }
        public string LastOficial { get; set; }
        public string NomeDoSistema { get; set; }
        public string UltimaAlteracao { get; set; }
        public string UltimaAlteracaoEncrypt { get; set; }
        public string UltimaCorrecao { get; set; }
        public string UltimaCorrecaoEncrypt { get; set; }
        public string UltimaEspecifica { get; set; }
        public string UltimaEspecificaEncrypt { get; set; }
        public string UltimaParalela { get; set; }
        public string UltimaParalelaEncrypt { get; set; }
        public string VersaoDb { get; set; }
        public string VersaoDoSistema { get; set; }
        public string VerticalSystem { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }        
    }
}