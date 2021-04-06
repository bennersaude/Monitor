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
using Monitor.Domain.Business.Commands.Endpoint;
using Monitor.Domain.ViewModels.Endpoint;

namespace Monitor.Api.Controllers
{
    [Authorize(Jwt.Bearer)]
    [Authorize(Roles = Roles.Administrador)]
    [ApiController]
    [Route("api/endpoint")]
    public class EndpointController : ControllerBase
    {
        private readonly IEndpointQuery EndpointQuery;
        private readonly IEndpointCommandHandler EndpointCommandHandler;
        private readonly IMapper mapper;

        public EndpointController(
            IEndpointQuery EndpointQuery,
            IEndpointCommandHandler EndpointCommandHandler,
            IMapper mapper)
        {
            this.EndpointQuery = EndpointQuery;
            this.EndpointCommandHandler = EndpointCommandHandler;
            this.mapper = mapper;
        }

        [HttpGet]
        [ValidateModelState]
        [OpenApiOperation(nameof(Listar), "Listar Endpoint")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(IEnumerable<DetalhesEndpointViewModel>))]
        public async Task<IEnumerable<DetalhesEndpointViewModel>> Listar()
        {
            return await EndpointQuery.ListarEndpointsAsync();
        }

        [HttpPost]
        [ValidateModelState]
        [OpenApiOperation(nameof(Incluir), "Incluir Endpoint")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesEndpointViewModel))]
        public async Task<DetalhesEndpointViewModel> Incluir(IncluirEndpointViewModel Endpoint)
        {
            return mapper.Map<DetalhesEndpointViewModel>(await
                EndpointCommandHandler.IncluirAsync(mapper.Map<IncluirEndpointCommand>(Endpoint)));
        }

        [HttpPut]
        [ValidateModelState]
        [OpenApiOperation(nameof(Alterar), "Alterar Endpoint")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesEndpointViewModel))]
        public async Task<DetalhesEndpointViewModel> Alterar(EditarEndpointViewModel Endpoint)
        {
            return mapper.Map<DetalhesEndpointViewModel>(await
                EndpointCommandHandler.EditarAsync(mapper.Map<EditarEndpointCommand>(Endpoint)));
        }

        [HttpDelete]
        [Route("{handle}")]
        [ValidateModelState]
        [OpenApiOperation(nameof(Excluir), "Excluir Endpoint")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(void))]
        public async Task Excluir(long handle)
        {
            await EndpointCommandHandler.ExcluirAsync(mapper.Map<ExcluirEndpointCommand>(handle));
        }
    }
}
