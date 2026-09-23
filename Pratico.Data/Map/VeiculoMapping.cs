using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class VeiculoMapping : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Placa)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Cor)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Modelo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Imagem)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");

            builder.ToTable("Veiculo");
        }
    }
}
