using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Data;
using Monitor.Data.NHibernate;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Sistema
{

    public class SistemaCommandHandler : SessionCommandHandler<SistemaCommand, Entities.Sistema>, ISistemaCommandHandler
    {
        public SistemaCommandHandler(ISessionProvider sessionProvider,
            IMapper mapper) : base(sessionProvider, mapper)
        {
        }

        public async Task<Entities.Sistema> IncluirAsync(IncluirSistemaCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var sistema = mapper.Map<Entities.Sistema>(command);
                sistema.CreateDateTime = DateTime.Now;
                session.SaveOrUpdate(sistema);
                return sistema;
            });
        }

        public async Task<Entities.Sistema> EditarAsync(EditarSistemaCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var sistemaFromCommand = mapper.Map<Entities.Sistema>(command);
                var sistema = session.Get<Entities.Sistema>(command.Handle);
                sistema.CopiarConfiguracoes(sistemaFromCommand);
                sistema.UpdateDateTime = DateTime.Now;
                return sistema;
            });
        }

        public async Task ExcluirAsync(ExcluirSistemaCommand command)
        {
            await base.ExecutarAsync(command, (c, session) =>
            {
                var sistema = session.Get<Entities.Sistema>(command.Handle);
                session.Delete(sistema);
            });
        }
    }
}