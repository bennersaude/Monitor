using System;

namespace Monitor.Domain.Business.Jobs.WebService
{
    public interface IPing
    {
         RespostaPing VerificaSeServicoWsdlDisponivel(Uri servicoUri);
         RespostaPing VerificaSeServicoDisponivel(Uri servicoUri);
    }
}