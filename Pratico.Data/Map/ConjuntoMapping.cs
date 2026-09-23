using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class ConjuntoMapping : IEntityTypeConfiguration<Conjunto>
    {
        public void Configure(EntityTypeBuilder<Conjunto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Descricao)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("Conjunto");
        }
    }
}
