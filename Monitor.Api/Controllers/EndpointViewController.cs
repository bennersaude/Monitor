using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Endpoint;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels;
using Monitor.Domain.ViewModels.Endpoint;

namespace Monitor.Api.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/endpointview")]
    public class EndpointViewController: Controller
    {
        private readonly IEndpointQuery endpointQuery;
        private readonly IEndpointCommandHandler endpointCommandHandler;
        private readonly IMapper mapper;

        public EndpointViewController(
            IEndpointQuery endpointQuery,
            IEndpointCommandHandler endpointCommandHandler,
            IMapper mapper)
        {
            this.endpointQuery = endpointQuery;
            this.endpointCommandHandler = endpointCommandHandler;
            this.mapper = mapper;
        }

        [Route("/endpointview/index/{handleSistema:long}")]
        public async Task<IActionResult> Index(long? handleSistema, [FromQuery] string nomeSistema)
        {
            if (handleSistema  == null)
            {
                return NotFound();
            }

            var endpoint = await endpointQuery.ListarEndpointsAsync(handleSistema.Value);
            if (endpoint == null)
                return NotFound();

            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);
        }

        [Route("/endpointview/details")]
        public async Task<IActionResult> Details(long? id, [FromQuery]long? handleSistema, [FromQuery]string nomeSistema)
        {
            if (id  == null)
            {
                return NotFound();
            }
            var endpoint = await endpointQuery.DetalhesEndpointAsync(id.Value);
            if (endpoint == null)
                return NotFound();
            
            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);
        }

        [Route("/endpointview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id, [FromQuery]long? handleSistema, [FromQuery]string nomeSistema)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endpoint = await endpointQuery.DetalhesEndpointAsync(id.Value);
            if (endpoint == null)
            {
                return NotFound();
            }
            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/endpointview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id, [FromForm] string nomeSistema,
          [Bind("Handle,HandleSistema,Nome,Url")]
          EditarEndpointViewModel endpoint)
        {
            if (id != endpoint.Handle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await endpointCommandHandler.EditarAsync(mapper.Map<EditarEndpointCommand>(endpoint));
                
                return RedirectToAction("Index", new {handleSistema = endpoint.HandleSistema, nomeSistema = nomeSistema});
            }

            ViewBag.handleSistema = endpoint.HandleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);
            
        }

        [HttpGet]
        [Route("/endpointview/create/{handleSistema:long}")]
        public IActionResult Create(long? handleSistema, [FromQuery]string nomeSistema)
        {
            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/endpointview/create/{handleSistema:long}")]
        public async Task<IActionResult> Create(long? handleSistema, [FromForm] string nomeSistema,
          [Bind("HandleSistema,Nome,Url")] 
          IncluirEndpointViewModel endpoint)
        {
            if (ModelState.IsValid)
            {
                await endpointCommandHandler.IncluirAsync(mapper.Map<IncluirEndpointCommand>(endpoint));
                
                return RedirectToAction("Index",new {handleSistema=handleSistema, nomeSistema = nomeSistema});
            }

            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);            
        }

        [Route("/endpointview/delete/{id:long}")]
        public async Task<IActionResult> Delete(long? id, [FromQuery]long? handleSistema, [FromQuery]string nomeSistema)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endpoint = await endpointQuery.DetalhesEndpointAsync(id.Value);
            if (endpoint == null)
            {
                return NotFound();
            }
            ViewBag.handleSistema = handleSistema;
            ViewBag.nomeSistema = nomeSistema;
            return View(endpoint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/endpointview/delete/{id:long}")]
        public async Task<IActionResult> DeleteConfirmed(long? id, [FromForm] long handleSistema, [FromForm] string nomeSistema)
        {
            if (id == null)
            {
                return NotFound();
            }

            await endpointCommandHandler.ExcluirAsync(mapper.Map<ExcluirEndpointCommand>(id.Value));
            return RedirectToAction(nameof(Index), new {handleSistema=handleSistema, nomeSistema = nomeSistema});
        }
        
    }
}