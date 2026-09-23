using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class SyncLogMapping : IEntityTypeConfiguration<SyncLog>
    {
        public void Configure(EntityTypeBuilder<SyncLog> builder)
        {
            builder.HasKey(p => p.Token);

            builder.Property(p => p.CondominioId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.ImovelId)
                .HasColumnType("uuid");

            builder.Property(p => p.Entidade)
                .IsRequired()
                .HasColumnType("varchar(120)");

            builder.Property(p => p.RegistroId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.Operacao)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.HasIndex(p => new { p.CondominioId, p.Token });

            builder.ToTable("SyncLog");
        }
    }
}
