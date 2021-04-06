using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Monitor.Data.NHibernate;
using Monitor.Domain.ViewModels;
using Monitor.Domain.Entities;
using System.Linq;
using Monitor.Data;
using Monitor.Domain.ViewModels.Ambiente;

namespace Monitor.Domain.Business.Queries
{
    public class AmbienteQuery : IAmbienteQuery
    {
        private readonly IMapper mapper;
        private readonly ISessionProvider sessionProvider;

        public AmbienteQuery(IMapper mapper, ISessionProvider sessionProvider)
        {
            this.mapper = mapper;
            this.sessionProvider = sessionProvider;
        }

        public async Task<IEnumerable<DetalhesAmbienteViewModel>> ListarAmbientesAsync()
        {
            using (var session = sessionProvider.OpenStatelessSession())
            {
                return await Task.FromResult(mapper.Map<IEnumerable<DetalhesAmbienteViewModel>>(session.Query<Ambiente>()));
            }
        }
    }
}