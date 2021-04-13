using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Sistema;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels;
using Monitor.Domain.ViewModels.Sistema;

namespace Monitor.Api.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/sistemaview")]
    public class SistemaViewController: Controller
    {
        private readonly ISistemaQuery sistemaQuery;
        private readonly ISistemaCommandHandler sistemaCommandHandler;
        private readonly IMapper mapper;

        public SistemaViewController(
            ISistemaQuery sistemaQuery,
            ISistemaCommandHandler sistemaCommandHandler,
            IMapper mapper)
        {
            this.sistemaQuery = sistemaQuery;
            this.sistemaCommandHandler = sistemaCommandHandler;
            this.mapper = mapper;
        }

        [Route("/sistemaview/index/{handleAmbiente:long}")]
        public async Task<IActionResult> Index(long? handleAmbiente, [FromQuery] string nomeAmbiente)
        {
            if (handleAmbiente  == null)
            {
                return NotFound();
            }

            var sistema = await sistemaQuery.ListarSistemasAsync(handleAmbiente.Value);
            if (sistema == null)
                return NotFound();

            ViewBag.handleAmbiente = handleAmbiente;
            ViewBag.nomeAmbiente = nomeAmbiente;
            return View(sistema);
        }

        [Route("/sistemaview/details")]
        public async Task<IActionResult> Details(long? id, [FromQuery]long? handleAmbiente, [FromQuery]string nomeAmbiente)
        {
            if (id  == null)
            {
                return NotFound();
            }
            var sistema = await sistemaQuery.DetalhesSistemaAsync(id.Value);
            if (sistema == null)
                return NotFound();
            
            ViewBag.handleAmbiente = handleAmbiente;
            ViewBag.nomeAmbiente = nomeAmbiente;
            return View(sistema);
        }

        [Route("/sistemaview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id, [FromQuery]long? handleAmbiente, [FromQuery]string nomeAmbiente)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sistema = await sistemaQuery.DetalhesSistemaAsync(id.Value);
            if (sistema == null)
            {
                return NotFound();
            }
            ViewBag.handleAmbiente = handleAmbiente;
            ViewBag.nomeAmbiente = nomeAmbiente;
            return View(sistema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/sistemaview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id, [FromForm] string nomeAmbiente,
          [Bind("Handle,HandleAmbiente,Nome,Cliente,Cnpj,MonitoramentoAtivo,UrlConsultaProcessos,UrlConsultaInformacoes")] 
          EditarSistemaViewModel sistema)
        {
            if (id != sistema.Handle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await sistemaCommandHandler.EditarAsync(mapper.Map<EditarSistemaCommand>(sistema));
                
                return RedirectToAction("Index", new {handleAmbiente = sistema.HandleAmbiente, nomeAmbiente = nomeAmbiente});
            }

            return View(sistema);
            
        }

        [HttpGet]
        [Route("/sistemaview/create/{handleAmbiente:long}")]
        public IActionResult Create(long? handleAmbiente, [FromQuery]string nomeAmbiente)
        {
            ViewBag.handleAmbiente = handleAmbiente;
            ViewBag.nomeAmbiente = nomeAmbiente;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/sistemaview/create/{handleAmbiente:long}")]
        public async Task<IActionResult> Create(long? handleAmbiente, [FromForm] string nomeAmbiente,
          [Bind("Handle,HandleAmbiente,Nome,Cliente,Cnpj,MonitoramentoAtivo,UrlConsultaProcessos,UrlConsultaInformacoes")] 
          IncluirSistemaViewModel sistema)
        {
            if (ModelState.IsValid)
            {
                await sistemaCommandHandler.IncluirAsync(mapper.Map<IncluirSistemaCommand>(sistema));
                
                return RedirectToAction("Index",new {handleAmbiente=handleAmbiente, nomeAmbiente = nomeAmbiente});
            }

            return View(sistema);            
        }

        [Route("/sistemaview/delete/{id:long}")]
        public async Task<IActionResult> Delete(long? id, [FromQuery]long? handleAmbiente, [FromQuery]string nomeAmbiente)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sistema = await sistemaQuery.DetalhesSistemaAsync(id.Value);
            if (sistema == null)
            {
                return NotFound();
            }
            ViewBag.handleAmbiente = handleAmbiente;
            ViewBag.nomeAmbiente = nomeAmbiente;
            return View(sistema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/sistemaview/delete/{id:long}")]
        public async Task<IActionResult> DeleteConfirmed(long? id, [FromForm] long handleAmbiente, [FromForm] string nomeAmbiente)
        {
            if (id == null)
            {
                return NotFound();
            }

            await sistemaCommandHandler.ExcluirAsync(mapper.Map<ExcluirSistemaCommand>(id.Value));
            return RedirectToAction(nameof(Index), new {handleAmbiente=handleAmbiente, nomeAmbiente = nomeAmbiente});
        }
        
    }
}