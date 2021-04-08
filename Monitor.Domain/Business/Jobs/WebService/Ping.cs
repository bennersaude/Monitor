using System;
using System.IO;
using System.Net;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public class Ping: IPing
    {
        private const string TIPOHTML = "text/html";
        private const string TAG_PARA_HABILITAR_WSDL = "httpGetEnabled";
        private const string FERRAMENTA_PARA_TESTAR_SERVICO = "svcutil.exe";

        private WebRequest webRequest;
        private IWebRequestCreate webRequestCreate;

        public Ping(IWebRequestCreate webRequestCreate)
        {
            this.webRequestCreate = webRequestCreate;
        }

        public virtual RespostaPing VerificaSeServicoWsdlDisponivel(Uri servicoUri)
        {
            var respostaPing = NovaRespostaPing(servicoUri);

            try
            {
                webRequest = webRequestCreate.Create(servicoUri);
                var response = (HttpWebResponse)webRequest.GetResponse();
                var responseStream = response.GetResponseStream();
                var conteudo = new StreamReader(responseStream).ReadToEnd();

                if (string.IsNullOrWhiteSpace(response.ContentType)
                    || response.StatusCode == HttpStatusCode.OK
                    && response.ContentType.Split(';')[0] == TIPOHTML
                    && !conteudo.Contains(TAG_PARA_HABILITAR_WSDL)
                    && !conteudo.Contains(FERRAMENTA_PARA_TESTAR_SERVICO))
                {
                    RespostaPingFalha(respostaPing);
                }
                else
                    RespostaPingSucesso(respostaPing, response);
            }
            catch (WebException e)
            {
                RespostaPingExcecao(respostaPing, e);
            }

            return respostaPing;
        }

        public virtual RespostaPing VerificaSeServicoDisponivel(Uri servicoUri)
        {
            var respostaPing = NovaRespostaPing(servicoUri);

            try
            {
                webRequest = webRequestCreate.Create(servicoUri);

                var response = (HttpWebResponse)webRequest.GetResponse();

                if (string.IsNullOrWhiteSpace(response.ContentType) || response.StatusCode != HttpStatusCode.OK)
                    RespostaPingFalha(respostaPing);
                else
                    RespostaPingSucesso(respostaPing, response);
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response?.StatusCode == HttpStatusCode.MethodNotAllowed)
                    RespostaPingSucessoQuandoStatus405(respostaPing);
                else
                    RespostaPingExcecao(respostaPing, e);
            }

            return respostaPing;
        }

        private RespostaPing NovaRespostaPing(Uri servicoUri)
        {
            var resposta = new RespostaPing();
            resposta.Url = servicoUri.AbsoluteUri;
            return resposta;
        }

        private void RespostaPingSucesso(RespostaPing respostaPing, HttpWebResponse response)
        {
            respostaPing.HttpStatus = (int)response.StatusCode;
            respostaPing.Mensagem = response.StatusDescription;
            respostaPing.ServicoAtivo = (int)response.StatusCode == 200;
        }

        private void RespostaPingSucessoQuandoStatus405(RespostaPing respostaPing)
        {
            respostaPing.HttpStatus = (int)HttpStatusCode.OK;
            respostaPing.Mensagem = "Sucesso!";
            respostaPing.ServicoAtivo = true;
        }

        private void RespostaPingFalha(RespostaPing respostaPing)
        {
            respostaPing.HttpStatus = (int)HttpStatusCode.NotFound;
            respostaPing.ServicoAtivo = false;
            respostaPing.Mensagem = HttpStatusCode.NotFound.ToString();
        }

        private void RespostaPingExcecao(RespostaPing respostaPing, WebException e)
        {
            respostaPing.Mensagem = e.Message;
            respostaPing.ServicoAtivo = false;
            respostaPing.HttpStatus = (int?)(e.Response as HttpWebResponse)?.StatusCode;
        }
    }
}