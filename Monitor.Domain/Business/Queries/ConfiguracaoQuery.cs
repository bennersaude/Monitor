using System.Linq;
using System.Threading.Tasks;
using Monitor.Domain.ViewModels.Configuracao;
using AutoMapper;
using Monitor.Data;
using Monitor.Domain.Entities;

namespace Monitor.Domain.Business.Queries
{
    public class ConfiguracaoQuery : IConfiguracaoQuery
    {
        private readonly IMapper mapper;
        private readonly ISessionProvider sessionProvider;

        public ConfiguracaoQuery(IMapper mapper, ISessionProvider sessionProvider)
        {
            this.mapper = mapper;
            this.sessionProvider = sessionProvider;
        }
        public async Task<DetalhesConfiguracaoViewModel> ObterConfiguracaoAsync()
        {
            using (var session = sessionProvider.OpenStatelessSession())
            {
                return await Task.FromResult(mapper.Map<DetalhesConfiguracaoViewModel>(
                    session.Get<Configuracao>((long)1)));
            }
        }        
    }
}