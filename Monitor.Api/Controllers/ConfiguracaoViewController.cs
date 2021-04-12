using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;

namespace Monitor.Api.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/configuracaoview")]
    public class ConfiguracaoViewController: Controller
    {
        private readonly IConfiguracaoQuery configuracaoQuery;
        private readonly IConfiguracaoCommandHandler configuracaoCommandHandler;
        private readonly IMapper mapper;

        public ConfiguracaoViewController(
            IConfiguracaoQuery configuracaoQuery,
            IConfiguracaoCommandHandler configuracaoCommandHandler,
            IMapper mapper)
        {
            this.configuracaoQuery = configuracaoQuery;
            this.configuracaoCommandHandler = configuracaoCommandHandler;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var configuracao = await configuracaoQuery.ObterConfiguracaoAsync();
            if (configuracao == null)
                return NotFound();

            return View(configuracao);
        }
        
    }
}