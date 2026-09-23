using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class OutBoxMessageMapping : IEntityTypeConfiguration<OutBoxMessage>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Tipo)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Conteudo)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasColumnType("varchar(30)")
                .HasDefaultValue(OutBoxMessageStatus.Pendente);

            builder.Property(p => p.Tentativas)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataUltimaTentativa)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataProcessamento)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.ProximaTentativaEm)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Erro)
                .HasColumnType("varchar(4000)");

            builder.HasIndex(p => new { p.Status, p.ProximaTentativaEm });

            builder.ToTable("OutBox");
        }
    }
}
