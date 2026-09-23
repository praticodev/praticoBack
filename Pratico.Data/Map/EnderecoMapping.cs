using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Bairro)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Cep)
                .HasColumnType("varchar(8)");

            builder.Property(p => p.Cidade)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Cobranca)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.Complemento)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Entidade)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Logradouro)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Numero)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Referencia)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.UF)
                .IsRequired()
                .HasColumnType("smallint");

            builder.ToTable("Endereco");
        }
    }
}
