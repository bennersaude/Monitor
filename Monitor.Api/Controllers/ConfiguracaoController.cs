using System.Threading.Tasks;
using AutoMapper;
using Monitor.Api.Attributes;
using Monitor.Api.Config;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Queries;
using Monitor.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Monitor.Domain.ViewModels.Configuracao;

namespace Monitor.Api.Controllers
{
    [Authorize(Jwt.Bearer)]
    [Authorize(Roles = Roles.Administrador)]
    [ApiController]
    [Route("api/configuracao")]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly IConfiguracaoQuery configuracaoQuery;
        private readonly IConfiguracaoCommandHandler configuracaoCommandHandler;
        private readonly IMapper mapper;

        public ConfiguracaoController(
            IConfiguracaoQuery configuracaoQuery,
            IConfiguracaoCommandHandler configuracaoCommandHandler,
            IMapper mapper)
        {
            this.configuracaoQuery = configuracaoQuery;
            this.configuracaoCommandHandler = configuracaoCommandHandler;
            this.mapper = mapper;
        }

        [HttpGet]
        [ValidateModelState]
        [OpenApiOperation(nameof(Obter), "Obter Configurações Gerais")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesConfiguracaoViewModel))]
        public async Task<DetalhesConfiguracaoViewModel> Obter()
        {
            return await configuracaoQuery.ObterConfiguracaoAsync();
        }

        [HttpPut]
        [ValidateModelState]
        [OpenApiOperation(nameof(Alterar), "Alterar Configurações Gerais")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(DetalhesConfiguracaoViewModel))]
        public async Task<DetalhesConfiguracaoViewModel> Alterar(EditarConfiguracaoViewModel configuracao)
        {
            return mapper.Map<DetalhesConfiguracaoViewModel>(await
                configuracaoCommandHandler.EditarAsync(mapper.Map<EditarConfiguracaoCommand>(configuracao)));
        }
    }
}
