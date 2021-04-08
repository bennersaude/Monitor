using AutoMapper;
using Monitor.ComponentModel;
using Monitor.Domain.ViewModels;
using Monitor.Domain.Entities;
using Monitor.Domain.Business.Commands.Ambiente;
using Monitor.Domain.Business.Commands.Configuracao;
using Monitor.Domain.ViewModels.Ambiente;
using Monitor.Domain.ViewModels.Configuracao;
using Monitor.Domain.ViewModels.Sistema;
using Monitor.Domain.Business.Commands.Sistema;
using Monitor.Domain.ViewModels.Endpoint;
using Monitor.Domain.Business.Commands.Endpoint;

namespace Monitor.Domain.Business.Commands
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<EditarConfiguracaoViewModel, EditarConfiguracaoCommand>(MemberList.None)
                .ConstructUsing(c => new EditarConfiguracaoCommand(c.IntervaloSegundosRecarregarAmbientes));

            MapearAmbiente();
            MapearSistema();
            MapearEndPoint();
        }

        private void MapearAmbiente()
        {
            CreateMap<IncluirAmbienteViewModel, IncluirAmbienteCommand>(MemberList.None)
                .ConstructUsing(c => new IncluirAmbienteCommand(c.Nome,
                    c.MonitoramentoAtivo, c.IntervaloSegundosWebServiceChecks,
                    c.TimeoutMilissegundosWebServiceChecks, c.QuantidadeChecagensConsiderarStatusWebService,
                    c.QuantidadeDiasExcluirDados));
            CreateMap<EditarAmbienteViewModel, EditarAmbienteCommand>(MemberList.None)
                .ConstructUsing(c => new EditarAmbienteCommand(c.Handle, c.Nome,
                c.MonitoramentoAtivo, c.IntervaloSegundosWebServiceChecks,
                    c.TimeoutMilissegundosWebServiceChecks, c.QuantidadeChecagensConsiderarStatusWebService,
                    c.QuantidadeDiasExcluirDados));
            CreateMap<long, ExcluirAmbienteCommand>(MemberList.None)
                .ConstructUsing(c => new ExcluirAmbienteCommand(c));
            CreateMap<IncluirAmbienteCommand, Entities.Ambiente>(MemberList.None);
            CreateMap<EditarAmbienteCommand, Entities.Ambiente>(MemberList.None);

        }

        private void MapearSistema()
        {
            CreateMap<IncluirSistemaViewModel, IncluirSistemaCommand>(MemberList.None)
                .ConstructUsing(c => new IncluirSistemaCommand(c.Nome,
                    c.MonitoramentoAtivo, c.HandleAmbiente,
                    c.Cliente, c.Cnpj, c.UrlConsultaProcessos, c.UrlConsultaInformacoes));
            CreateMap<EditarSistemaViewModel, EditarSistemaCommand>(MemberList.None)
                .ConstructUsing(c => new EditarSistemaCommand(c.Handle, c.Nome,
                    c.MonitoramentoAtivo, c.HandleAmbiente,
                    c.Cliente, c.Cnpj, c.UrlConsultaProcessos, c.UrlConsultaInformacoes));
            CreateMap<long, ExcluirSistemaCommand>(MemberList.None)
                .ConstructUsing(c => new ExcluirSistemaCommand(c));            
            CreateMap<IncluirSistemaCommand, Entities.Sistema>(MemberList.None)
                .ForPath(dest => dest.Ambiente.Handle, opt => opt.MapFrom(src => src.HandleAmbiente));
            CreateMap<EditarSistemaCommand, Entities.Sistema>(MemberList.None);

        }

        private void MapearEndPoint()
        {
            CreateMap<IncluirEndpointViewModel, IncluirEndpointCommand>(MemberList.None)
                .ConstructUsing(c => new IncluirEndpointCommand(c.Nome,
                    c.Url, c.HandleSistema));
            CreateMap<EditarEndpointViewModel, EditarEndpointCommand>(MemberList.None)
                .ConstructUsing(c => new EditarEndpointCommand(c.Handle, c.Nome,
                    c.Url, c.HandleSistema));
            CreateMap<long, ExcluirEndpointCommand>(MemberList.None)
                .ConstructUsing(c => new ExcluirEndpointCommand(c));            
            CreateMap<IncluirEndpointCommand, Entities.Endpoint>(MemberList.None)
                .ForPath(dest => dest.Sistema.Handle, opt => opt.MapFrom(src => src.HandleSistema));
            CreateMap<EditarEndpointCommand, Entities.Endpoint>(MemberList.None);

        }
    }
}