using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Data;
using Monitor.Data.NHibernate;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Endpoint
{

    public class EndpointCommandHandler : SessionCommandHandler<EndpointCommand, Entities.Endpoint>, IEndpointCommandHandler
    {
        public EndpointCommandHandler(ISessionProvider sessionProvider,
            IMapper mapper) : base(sessionProvider, mapper)
        {
        }

        public async Task<Entities.Endpoint> IncluirAsync(IncluirEndpointCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var Endpoint = mapper.Map<Entities.Endpoint>(command);
                Endpoint.CreateDateTime = DateTime.Now;
                session.SaveOrUpdate(Endpoint);
                return Endpoint;
            });
        }

        public async Task<Entities.Endpoint> EditarAsync(EditarEndpointCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var EndpointFromCommand = mapper.Map<Entities.Endpoint>(command);
                var Endpoint = session.Get<Entities.Endpoint>(command.Handle);
                Endpoint.CopiarConfiguracoes(EndpointFromCommand);
                Endpoint.UpdateDateTime = DateTime.Now;
                return Endpoint;
            });
        }

        public async Task ExcluirAsync(ExcluirEndpointCommand command)
        {
            await base.ExecutarAsync(command, (c, session) =>
            {
                var Endpoint = session.Get<Entities.Endpoint>(command.Handle);
                session.Delete(Endpoint);
            });
        }
    }
}