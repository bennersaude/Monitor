using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels.Configuracao;

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

        [Route("/configuracaoview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracao = await configuracaoQuery.ObterConfiguracaoAsync();
            if (configuracao == null)
            {
                return NotFound();
            }
            return View(configuracao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/configuracaoview/edit/{id:int}")]
        public async Task<IActionResult> Edit(long? id, 
          [Bind("IntervaloSegundosRecarregarAmbientes")] 
          EditarConfiguracaoViewModel configuracao)
        {

            if (ModelState.IsValid)
            {
                await configuracaoCommandHandler.EditarAsync(mapper.Map<EditarConfiguracaoCommand>(configuracao));
                
                return RedirectToAction("Index");
            }

            return View(configuracao);
            
        }
        
    }
}