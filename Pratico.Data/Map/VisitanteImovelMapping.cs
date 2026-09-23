using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class VisitanteImovelMapping : IEntityTypeConfiguration<VisitanteImovel>
    {
        public void Configure(EntityTypeBuilder<VisitanteImovel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Email)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Rg)
                .HasColumnType("varchar(30)");

            builder.Property(p => p.Cpf)
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Celular)
                .IsRequired()
                .HasColumnType("varchar(15)");

            builder.Property(p => p.Imagem)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("VisitanteImovel");
        }
    }
}
