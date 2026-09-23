using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class AgendaAreaLazerMapping : IEntityTypeConfiguration<AgendaAreaLazer>
    {
        public void Configure(EntityTypeBuilder<AgendaAreaLazer> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AreaLazerId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.PessoaId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.DataInicio)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataFim)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Autorizado)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("AgendaAreaLazer");
        }
    }
}
