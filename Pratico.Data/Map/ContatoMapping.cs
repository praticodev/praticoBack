using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class ContatoMapping : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Ddd)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Entidade)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Principal)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.Telefone)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'");

            builder.ToTable("Contato");
        }
    }
}
