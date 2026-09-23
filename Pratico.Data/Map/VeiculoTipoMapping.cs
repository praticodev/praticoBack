using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class VeiculoTipoMapping : IEntityTypeConfiguration<VeiculoTipo>
    {
        public void Configure(EntityTypeBuilder<VeiculoTipo> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("VeiculoTipo");
        }
    }
}
