using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Api.Attributes;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Ambiente;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Monitor.Domain.ViewModels.Ambiente;

namespace Monitor.Api.Controllers
{
    [Authorize(Jwt.Bearer)]
    [Authorize(Roles = Roles.Administrador)]
    [ApiController]
    [Route("api/ambiente")]
    public class AmbienteController : ControllerBase
    {
        private readonly IAmbienteQuery ambienteQuery;
        private readonly IAmbienteCommandHandler ambienteCommandHandler;
        private readonly IMapper mapper;

        public AmbienteController(
            IAmbienteQuery ambienteQuery,
            IAmbienteCommandHandler ambienteCommandHandler,
            IMapper mapper)
        {
            this.ambienteQuery = ambienteQuery;
            this.ambienteCommandHandler = ambienteCommandHandler;
            this.mapper = mapper;
        }

        [HttpGet]
        [ValidateModelState]
        [OpenApiOperation(nameof(Listar), "Listar Ambientes")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(IEnumerable<DetalhesAmbienteViewModel>))]
        public async Task<IEnumerable<DetalhesAmbienteViewModel>> Listar()
        {
            return await ambienteQuery.ListarAmbientesAsync();
        }

        [HttpPost]
        [ValidateModelState]
        [OpenApiOperation(nameof(Incluir), "Incluir Ambiente")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesAmbienteViewModel))]
        public async Task<DetalhesAmbienteViewModel> Incluir(IncluirAmbienteViewModel ambiente)
        {
            return mapper.Map<DetalhesAmbienteViewModel>(await
                ambienteCommandHandler.IncluirAsync(mapper.Map<IncluirAmbienteCommand>(ambiente)));
        }

        [HttpPut]
        [ValidateModelState]
        [OpenApiOperation(nameof(Alterar), "Alterar Ambiente")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesAmbienteViewModel))]
        public async Task<DetalhesAmbienteViewModel> Alterar(EditarAmbienteViewModel ambiente)
        {
            return mapper.Map<DetalhesAmbienteViewModel>(await
                ambienteCommandHandler.EditarAsync(mapper.Map<EditarAmbienteCommand>(ambiente)));
        }

        [HttpDelete]
        [Route("{handle}")]
        [ValidateModelState]
        [OpenApiOperation(nameof(Excluir), "Excluir Ambiente")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(void))]
        public async Task Excluir(long handle)
        {
            await ambienteCommandHandler.ExcluirAsync(mapper.Map<ExcluirAmbienteCommand>(handle));
        }
    }
}
