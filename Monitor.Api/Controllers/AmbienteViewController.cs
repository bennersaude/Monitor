using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Ambiente;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels;
using Monitor.Domain.ViewModels.Ambiente;

namespace Monitor.Api.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/ambienteview")]
    public class AmbienteViewController: Controller
    {
        private readonly IAmbienteQuery ambienteQuery;
        private readonly IAmbienteCommandHandler ambienteCommandHandler;
        private readonly IMapper mapper;

        public AmbienteViewController(
            IAmbienteQuery ambienteQuery,
            IAmbienteCommandHandler ambienteCommandHandler,
            IMapper mapper)
        {
            this.ambienteQuery = ambienteQuery;
            this.ambienteCommandHandler = ambienteCommandHandler;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var ambiente = await ambienteQuery.ListarAmbientesAsync();
            if (ambiente == null)
                return NotFound();

            return View(ambiente);
        }

        [Route("/ambienteview/details")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id  == null)
            {
                return NotFound();
            }
            var ambiente = await ambienteQuery.DetalhesAmbienteAsync(id.Value);
            if (ambiente == null)
                return NotFound();

            return View(ambiente);
        }

        [Route("/ambienteview/edit/{id:long}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await ambienteQuery.DetalhesAmbienteAsync(id.Value);
            if (ambiente == null)
            {
                return NotFound();
            }
            return View(ambiente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/ambienteview/edit/{id:int}")]
        public async Task<IActionResult> Edit(long? id, 
          [Bind("Handle,Nome,MonitoramentoAtivo,IntervaloSegundosWebServiceChecks, TimeoutMilissegundosWebServiceChecks,QuantidadeChecagensConsiderarStatusWebService,QuantidadeDiasExcluirDados")] 
          EditarAmbienteViewModel ambiente)
        {
            if (id != ambiente.Handle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await ambienteCommandHandler.EditarAsync(mapper.Map<EditarAmbienteCommand>(ambiente));
                
                return RedirectToAction("Index");
            }

            return View(ambiente);
            
        }

        [Route("/ambienteview/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/ambienteview/create")]
        public async Task<IActionResult> Create( 
          [Bind("Nome,MonitoramentoAtivo,IntervaloSegundosWebServiceChecks, TimeoutMilissegundosWebServiceChecks,QuantidadeChecagensConsiderarStatusWebService,QuantidadeDiasExcluirDados")] 
          IncluirAmbienteViewModel ambiente)
        {
            if (ModelState.IsValid)
            {
                await ambienteCommandHandler.IncluirAsync(mapper.Map<IncluirAmbienteCommand>(ambiente));
                
                return RedirectToAction("Index");
            }

            return View(ambiente);            
        }

        [Route("/ambienteview/delete/{id:int}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ambiente = await ambienteQuery.DetalhesAmbienteAsync(id.Value);
            if (ambiente == null)
            {
                return NotFound();
            }
            return View(ambiente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/ambienteview/delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await ambienteCommandHandler.ExcluirAsync(mapper.Map<ExcluirAmbienteCommand>(id.Value));
            return RedirectToAction(nameof(Index));
        }
        
    }
}