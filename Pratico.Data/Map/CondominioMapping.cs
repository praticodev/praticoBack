using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class CondominioMapping : IEntityTypeConfiguration<Condominio>
    {
        public void Configure(EntityTypeBuilder<Condominio> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.RazaoSocial)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NomeFantasia)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Cnpj)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(p => p.Ativo)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DiaVencimento)
                .HasColumnType("smallint");

            builder.Property(p => p.Finalidade)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Fracao)
                .IsRequired()
                .HasColumnType("decimal");

            builder.Property(p => p.Area)
                //.IsRequired()
                .HasColumnType("decimal");

            builder.Property(p => p.CodCondominio)
                .HasColumnType("int");

            builder.Property(p => p.InscEstadual)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.InscMunicipal)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.TipoCondominio)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.UsuarioId)
                .HasColumnType("uuid")
                .IsRequired();

            // 1 : N => Condominio : Conjuntos
            builder.HasMany(f => f.Conjuntos)
                .WithOne(p => p.Condominio)
                .HasForeignKey(p => p.CondominioId);

            builder.HasMany(f => f.Remessas)
                .WithOne(p => p.Condominio)
                .HasForeignKey(p => p.CondominioId);

            builder.ToTable("Condominio");
        }
    }
}
