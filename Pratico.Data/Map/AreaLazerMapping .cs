using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class AreaLazerMapping : IEntityTypeConfiguration<AreaLazer>
    {
        public void Configure(EntityTypeBuilder<AreaLazer> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Capacidade)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("AreaLazer");
        }
    }
}
