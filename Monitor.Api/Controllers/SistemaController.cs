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
using Monitor.Domain.Business.Commands.Sistema;
using Monitor.Domain.ViewModels.Sistema;

namespace Monitor.Api.Controllers
{
    [Authorize(Jwt.Bearer)]
    [Authorize(Roles = Roles.Administrador)]
    [ApiController]
    [Route("api/sistema")]
    public class SistemaController : ControllerBase
    {
        private readonly ISistemaQuery sistemaQuery;
        private readonly ISistemaCommandHandler sistemaCommandHandler;
        private readonly IMapper mapper;

        public SistemaController(
            ISistemaQuery sistemaQuery,
            ISistemaCommandHandler sistemaCommandHandler,
            IMapper mapper)
        {
            this.sistemaQuery = sistemaQuery;
            this.sistemaCommandHandler = sistemaCommandHandler;
            this.mapper = mapper;
        }

        [HttpGet]
        [ValidateModelState]
        [OpenApiOperation(nameof(Listar), "Listar Sistema")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(IEnumerable<DetalhesSistemaViewModel>))]
        public async Task<IEnumerable<DetalhesSistemaViewModel>> Listar()
        {
            return await sistemaQuery.ListarSistemasAsync();
        }

        [HttpPost]
        [ValidateModelState]
        [OpenApiOperation(nameof(Incluir), "Incluir Sistema")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesSistemaViewModel))]
        public async Task<DetalhesSistemaViewModel> Incluir(IncluirSistemaViewModel Sistema)
        {
            return mapper.Map<DetalhesSistemaViewModel>(await
                sistemaCommandHandler.IncluirAsync(mapper.Map<IncluirSistemaCommand>(Sistema)));
        }

        [HttpPut]
        [ValidateModelState]
        [OpenApiOperation(nameof(Alterar), "Alterar Sistema")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesSistemaViewModel))]
        public async Task<DetalhesSistemaViewModel> Alterar(EditarSistemaViewModel Sistema)
        {
            return mapper.Map<DetalhesSistemaViewModel>(await
                sistemaCommandHandler.EditarAsync(mapper.Map<EditarSistemaCommand>(Sistema)));
        }

        [HttpDelete]
        [Route("{handle}")]
        [ValidateModelState]
        [OpenApiOperation(nameof(Excluir), "Excluir Sistema")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(void))]
        public async Task Excluir(long handle)
        {
            await sistemaCommandHandler.ExcluirAsync(mapper.Map<ExcluirSistemaCommand>(handle));
        }
    }
}
