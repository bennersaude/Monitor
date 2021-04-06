using System;
using Monitor.Domain.Helper;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public class ConectividadeServico: IConectividadeServico
    {
        private readonly IPing ping;

        public ConectividadeServico(IPing ping)
        {
            this.ping = ping;
        }

        public bool ServicoEstaAtivo(string enderecoServico)
        {
            var respostaPing = TestaConectividade(enderecoServico);
            return respostaPing.ServicoAtivo;
        }

        public RespostaPing TestaConectividade(string enderecoServico)
        {
            if (UriHelper.IsService(enderecoServico))
                return ping.VerificaSeServicoWsdlDisponivel(CriaWsdlUri(enderecoServico));

            return ping.VerificaSeServicoDisponivel(new Uri(enderecoServico));
        }

        private static Uri CriaWsdlUri(string servicoUrl)
        {
            var wsdlUrl = servicoUrl+"?wsdl";
            return new Uri(wsdlUrl);
        }

    }
}