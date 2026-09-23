using AutoMapper;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Condominio, CondominioViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<RepresentanteLegal, RepresentanteLegalViewModel>()
                .ForMember(dest => dest.DataNascimentoRep, opt => opt.MapFrom(src => Convert.ToDateTime(src.DataNascimentoRep)))
                .ForMember(dest => dest.IniPeriodo, opt => opt.MapFrom(src => Convert.ToDateTime(src.IniPeriodo)))
                .ForMember(dest => dest.FimPeriodo, opt => opt.MapFrom(src => Convert.ToDateTime(src.FimPeriodo)));
            CreateMap<RepresentanteLegalViewModel, RepresentanteLegal>()
                .ForMember(dest => dest.DataNascimentoRep, opt => opt.MapFrom(src => String.Format("{0:yyyy-MM-dd}", src.DataNascimentoRep)))
                .ForMember(dest => dest.IniPeriodo, opt => opt.MapFrom(src => String.Format("{0:yyyy-MM-dd}", src.IniPeriodo)))
                .ForMember(dest => dest.FimPeriodo, opt => opt.MapFrom(src => String.Format("{0:yyyy-MM-dd}", src.FimPeriodo)));
            CreateMap<Conjunto, ConjuntoViewModel>().ReverseMap();
            CreateMap<Andar, AndarViewModel>().ReverseMap();
            CreateMap<Contato, ContatoCondominioViewModel>().ReverseMap();
            CreateMap<CondominioCompleto, CondominioCompletoViewModel>().ReverseMap();
            CreateMap<GerenciamentoDocImovel, DocImovelViewModel>().ReverseMap();
            CreateMap<ImovelViewModel, Imovel>()
                .ForMember(dest => dest.Fracao, opt => opt.MapFrom(src => double.Parse(src.Fracao)))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => double.Parse(src.Area)));
            CreateMap<Imovel, ImovelViewModel>()
                .ForMember(dest => dest.Fracao, opt => opt.MapFrom(src => src.Fracao.ToString()))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.ToString()));
            CreateMap<DocumentoImovel, DocumentoImovelViewModel>().ReverseMap();
            CreateMap<ImovelCondominio, ImovelCondominioViewModel>().ReverseMap();
            CreateMap<ImoveisProprietario, ImoveisProprietarioViewModel>().ReverseMap();
            CreateMap<Proprietario, DadosPropsViewModel>().ReverseMap();
            CreateMap<Proprietario, UsuarioMoradorViewModel>().ReverseMap();
            CreateMap<Inquilino, DadosPropsViewModel>().ReverseMap();
            CreateMap<DocumentoImovel, ImoveDocViewModel>().ReverseMap();
            CreateMap<ProprietarioEndereco, DadosPropsViewModel>().ReverseMap();
            CreateMap<EmpresaSimplificada, EmpresaSimplificadaViewModel>().ReverseMap();
            CreateMap<Morador, MoradorViewModel>().ReverseMap();
            CreateMap<RemessaExterna, RemessaExternaViewModel>().ReverseMap();
            CreateMap<RemessaInterna, RemessaInternaViewModel>().ReverseMap();
            CreateMap<TipoDocumentoEntrega, TipoDocumentoEntregaViewModel>().ReverseMap();
            CreateMap<MoradorCorrespondencia, MoradorCorrespondeciaViewModel>().ReverseMap();
            CreateMap<Operador, RegisterOperadorViewModel>().ReverseMap();
            CreateMap<VeiculoFabricante, VeiculoFabricanteViewModel>().ReverseMap();
            CreateMap<VeiculoCor, VeiculoCorViewModel>().ReverseMap();
            CreateMap<VeiculoTipo, VeiculoTipoViewModel>().ReverseMap();
            CreateMap<VeiculoViewModel, Veiculo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Veiculo, VeiculoViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Modelo));
            CreateMap<VeiculoAppViewModel, Veiculo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Veiculo, VeiculoAppViewModel>();
            CreateMap<TipoPrestadorServico, TipoPrestadorServicoViewModel>().ReverseMap();
            CreateMap<PrestadorServico, PrestadorServicoViewModel>().ReverseMap();
            CreateMap<VisitanteImovel, VisitanteImovelViewModel>().ReverseMap();
            //var tmp = representante.DataNascimentoRep.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("pt-BR"));
        }
    }
}
