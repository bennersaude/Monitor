using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Data;
using Monitor.Data.NHibernate;
using Monitor.Domain.Business.Commands;
using Monitor.Domain.Entities;
using NHibernate;

namespace Monitor.Domain.Business.Commands.Ambiente
{

    public class AmbienteCommandHandler : SessionCommandHandler<AmbienteCommand, Entities.Ambiente>, IAmbienteCommandHandler
    {
        public AmbienteCommandHandler(ISessionProvider sessionProvider,
            IMapper mapper) : base(sessionProvider, mapper)
        {
        }

        public async Task<Entities.Ambiente> IncluirAsync(IncluirAmbienteCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var ambiente = mapper.Map<Entities.Ambiente>(command);
                ambiente.CreateDateTime = DateTime.Now;
                session.SaveOrUpdate(ambiente);
                return ambiente;
            });
        }

        public async Task<Entities.Ambiente> EditarAsync(EditarAmbienteCommand command)
        {
            return await base.ExecutarERetornarAsync(command, (c, session) =>
            {
                var ambienteFromCommand = mapper.Map<Entities.Ambiente>(command);
                var ambiente = session.Get<Entities.Ambiente>(command.Handle);
                ambiente.CopiarConfiguracoes(ambienteFromCommand);
                ambiente.UpdateDateTime = DateTime.Now;
                return ambiente;
            });
        }

        public async Task ExcluirAsync(ExcluirAmbienteCommand command)
        {
            await base.ExecutarAsync(command, (c, session) =>
            {
                var ambiente = session.Get<Entities.Ambiente>(command.Handle);
                session.Delete(ambiente);
            });
        }
    }
}