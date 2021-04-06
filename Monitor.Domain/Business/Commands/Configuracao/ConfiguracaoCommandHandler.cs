using System;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Data;
using Monitor.Data.NHibernate;

namespace Monitor.Domain.Business.Commands.Configuracao
{
    public class ConfiguracaoCommandHandler : SessionCommandHandler<ConfiguracaoCommand, Entities.Configuracao>, IConfiguracaoCommandHandler
    {
        public ConfiguracaoCommandHandler(ISessionProvider sessionProvider,
            IMapper mapper) : base(sessionProvider, mapper)
        {
        }

        public async Task<Entities.Configuracao> EditarAsync(EditarConfiguracaoCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var configuracao = session.Get<Entities.Configuracao>(Entities.Configuracao.HANDLE_FIXO);
                configuracao.IntervaloSegundosRecarregarAmbientes = command.IntervaloSegundosRecarregarAmbientes;
                configuracao.UpdateDateTime = DateTime.Now;
                session.SaveOrUpdate(configuracao);
                return configuracao;
            });
        }
    }
}