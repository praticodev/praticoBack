using Microsoft.EntityFrameworkCore;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pratico.Data.Context
{
    public class PraticoContext : DbContext
    {
        public PraticoContext(DbContextOptions<PraticoContext> options) : base(options) { }

        public DbSet<Condominio> Condominio { get; set; }
        public DbSet<Contato> Contato { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<RepresentanteLegal> RepresentanteLegal { get; set; }
        public DbSet<Conjunto> Conjunto { get; set; }
        public DbSet<Andar> Andar { get; set; }
        public DbSet<Imovel> Imovel { get; set; }
        public DbSet<GerenciamentoDocImovel> GerenciamentoDocImovel { get; set; }
        public DbSet<DocumentoImovel> DocumentoImovel { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<MoradorImovel> MoradorImovel { get; set; }
        public DbSet<UsuarioCondominio> UsuarioCondominio { get; set; }
        public DbSet<InquilinoImovel> InquilinoImovel { get; set; }
        public DbSet<ProprietarioImovel> ProprietarioImovel { get; set; }
        public DbSet<PessoaUsuario> PessoaUsuario { get; set; }
        public DbSet<EmpresaSimplificada> EmpresaSimplificada { get; set; }
        public DbSet<RemessaExterna> RemessaExterna { get; set; }
        public DbSet<RemessaInterna> RemessaInterna { get; set; }
        public DbSet<TipoFormato> TipoFormato { get; set; }
        public DbSet<TipoItem> TipoItem { get; set; }
        public DbSet<TipoVolume> TipoVolume { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<TipoAreaLazer> TipoAreaLazer { get; set; }
        public DbSet<AgendaAreaLazer> AgendaAreaLazer { get; set; }
        public DbSet<AreaLazer> AreaLazer { get; set; }
        public DbSet<FilaEspera> FilaEspera { get; set; }
        public DbSet<ConvidadoEvento> ConvidadoEvento { get; set; }
        public DbSet<ListaEvento> ListaEvento { get; set; }
        public DbSet<AcessoVisitante> AcessoVisitante { get; set; }
        public DbSet<UsuarioSistema> UsuarioSistema { get; set; }
        public DbSet<Operador> Operador { get; set; }
        public DbSet<VeiculoFabricante> VeiculoFabricante { get; set; }
        public DbSet<VeiculoCor> VeiculoCor { get; set; }
        public DbSet<VeiculoTipo> VeiculoTipo { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<TipoPrestadorServico> TipoPrestadorServico { get; set; }
        public DbSet<PrestadorServico> PrestadorServico { get; set; }
        public DbSet<VisitanteImovel> VisitanteImovel { get; set; }
        public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
        public DbSet<SyncLog> SyncLog { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PraticoContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            NormalizarDateTimesUtc();

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataAtualizacao") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataAtualizacao").IsModified = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataAtualizacao").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void NormalizarDateTimesUtc()
        {
            foreach (var entry in ChangeTracker.Entries()
                         .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue is DateTime dateTime &&
                        dateTime.Kind == DateTimeKind.Utc)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                    }
                }
            }
        }
    }
}
