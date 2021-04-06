using System;
using Microsoft.Extensions.DependencyInjection;
using Monitor.Domain.Business.Commands.Ambiente;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.Business.Commands.Endpoint;
using Monitor.Domain.Business.Commands.Sistema;
using Monitor.Domain.Business.Jobs;
using Monitor.Domain.Business.Queries;

namespace Monitor.Domain
{
    public class DependencyInjectionHelper
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IConfiguracaoQuery, ConfiguracaoQuery>();
            services.AddScoped<IConfiguracaoCommandHandler, ConfiguracaoCommandHandler>();
            services.AddScoped<IJobsExecuter, JobsExecuter>();
            services.AddScoped<IAmbienteQuery, AmbienteQuery>();
            services.AddScoped<IAmbienteCommandHandler, AmbienteCommandHandler>();
            services.AddScoped<ISistemaQuery, SistemaQuery>();
            services.AddScoped<ISistemaCommandHandler, SistemaCommandHandler>();
            services.AddScoped<IEndpointQuery, EndpointQuery>();
            services.AddScoped<IEndpointCommandHandler, EndpointCommandHandler>();
        }
    }
}
