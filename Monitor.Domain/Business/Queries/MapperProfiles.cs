using AutoMapper;
using Monitor.ComponentModel;
using Monitor.Domain.Entities;
using Monitor.Domain.ViewModels.Ambiente;
using Monitor.Domain.ViewModels.Configuracao;
using Monitor.Domain.ViewModels.Endpoint;
using Monitor.Domain.ViewModels.Entidade;
using Monitor.Domain.ViewModels.Sistema;

namespace Monitor.Domain.Business.Queries
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Entidade, EntidadeViewModel>(MemberList.None);
            CreateMap<EntidadeAuditavel, EntidadeAuditavelViewModel>(MemberList.None);
            CreateMap<Ambiente, DetalhesAmbienteViewModel>(MemberList.None);
            CreateMap<Configuracao, DetalhesConfiguracaoViewModel>(MemberList.None);
            CreateMap<Sistema, DetalhesSistemaViewModel>(MemberList.None)            
                .ForMember(dest => dest.HandleAmbiente, opt => opt.MapFrom(src => src.Ambiente.Handle));
            CreateMap<Endpoint, DetalhesEndpointViewModel>(MemberList.None)            
                .ForMember(dest => dest.HandleSistema, opt => opt.MapFrom(src => src.Sistema.Handle));
        }
    }
}