using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class TipoPrestadorServicoMapping : IEntityTypeConfiguration<TipoPrestadorServico>
    {
        public void Configure(EntityTypeBuilder<TipoPrestadorServico> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("TipoPrestadorServico");
        }
    }
}
