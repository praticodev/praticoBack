using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pratico.Api.Extensions;
using Pratico.Business.Services;
using Pratico.Business.Utils;
using Pratico.Data.Context;
using Pratico.Data.Repository;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Notificacoes;
using System.Reflection;

namespace Pratico.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<PraticoContext>();
            services.AddScoped<ICondominioRepository, CondominioRepository>();
            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IRepresentanteLegalRepository, RepresentanteLegalRepository>();
            services.AddScoped<IConjuntoRepository, ConjuntoRepository>();
            services.AddScoped<IAndarRepository, AndarRepository>(); 
            services.AddScoped<IImovelRepository, ImovelRepository>();
            services.AddScoped<IDocImovelRepository, DocImovelRepository>();
            services.AddScoped<IProprietarioRepository, ProprietarioRepository>();
            services.AddScoped<IDocumentoImovelRepository, DocumentoImovelRepository>();
            services.AddScoped<IInquilinoRepository, InquilinoRepository>();
            services.AddScoped<IInquilinoImovelRepository, InquilinoImovelRepository>();
            services.AddScoped<IMoradorImovelRepository, MoradorImovelRepository>();
            services.AddScoped<IProprietarioImovelRepository, ProprietarioImovelRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IUsuarioCondominioRepository, MoradorCondominiolRepository>();
            services.AddScoped<IMoradorRepository, MoradorRepository>();
            services.AddScoped<IPessoaUsuarioRepository, PessoaUsuarioRepository>();
            services.AddScoped<IEmpresaSimplificadaRepository, EmpresaSimplificadaRepository>();
            services.AddScoped<IRemessaRepository, RemessaRepository>();
            services.AddScoped<IRemessaInternaRepository, RemessaInternaRepository>();
            services.AddScoped<ITipoDocumentoEntregaRepository, TipoDocumentoEntregaRepository>();
            services.AddScoped<ITipoAreaLazerRepository, TipoAreaLazerRepository>();
            services.AddScoped<IAgendaAreaLazerRepository, AgendaAreaLazerRepository>();
            services.AddScoped<IAreaLazerRepository, AreaLazerRepository>();
            services.AddScoped<IFilaEsperaRepository, FilaEsperaRepository>();
            services.AddScoped<IListaEventoRepository, ListaEventoRepository>();
            services.AddScoped<IAcessoVisitanteRepository, AcessoVisitanteRepository>();
            services.AddScoped<IUsuarioSistemaRepository, UsuarioSistemaRepository>();
            services.AddScoped<IOperadorRepository, OperadorRepository>();
            services.AddScoped<IRelatorioCorrespondenciaRepository, RelatorioCorrespondenciaRepository>();
            services.AddScoped<IVeiculoFabricanteRepository, VeiculoFrabricanteRepository>();
            services.AddScoped<IVeiculoCorRepository, VeiculoCorRepository>();
            services.AddScoped<IVeiculoTipoRepository, VeiculoTipoRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IPrestadorServicoRepository, PrestadorServicoRepository>();
            services.AddScoped<IVisitanteImovelRepository, VisitanteImovelRepository>();
            services.AddScoped<ISyncLogRepository, SyncLogRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<ICondominioService, CondominioService>();
            services.AddScoped<IContatoService, ContatoService>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<IRepresentanteLegalService, RepresentanteLegalService>();
            services.AddScoped<IConjuntoService, ConjuntoService>();
            services.AddScoped<IAndarService, AndarService>();
            services.AddScoped<IDocImovelService, DocImovelService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProprietarioService, ProprietarioService>();
            services.AddScoped<IImovelService, ImovelService>();
            services.AddScoped<IInquilinoService, InquilinoService>();
            services.AddScoped<IUsuarioCondominioService, UsuarioCondominioService>();
            services.AddScoped<IMoradorService, MoradorService>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<IPessoaUsuarioService, PessoaUsuarioService>();
            services.AddScoped<IProprietarioImovelService, ProprietarioImovelService>();
            services.AddScoped<IEmpresaSimplificadaService, EmpresaSimplificadaService>();
            services.AddScoped<IRemessaService, RemessaService>();
            services.AddScoped<IEmailSenGridService, EmailSenGridService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IAreaLazerService, AreaLazerService>();
            services.AddScoped<IReversaService, ReversaService>();
            services.AddScoped<IUsuarioSistemaService, UsuarioSistemaService>();
            services.AddScoped<IOperadorService, OperadorService>();
            services.AddScoped<IRelatorioCorrespondenciaService, RelatorioCorrespondenciaService>();
            services.AddScoped<IVeiculoFabricanteService, VeiculoFabricanteService>();
            services.AddScoped<IVeiculoCorService, VeiculoCorService>();
            services.AddScoped<IVeiculoTipoService, VeiculoTipoService>();
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IPrestadorServicoService, PrestadorServicoService>();
            services.AddScoped<IVisitanteImovelService, VisitanteImovelService>();
            services.AddScoped<ISyncLogService, SyncLogService>();

            

            return services;
        }
    }
}
