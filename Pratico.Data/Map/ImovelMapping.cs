using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class ImovelMapping : IEntityTypeConfiguration<Imovel>
    {
        public void Configure(EntityTypeBuilder<Imovel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Conjunto)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.NumAndar)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.NumAndar)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.NumImovel)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(p => p.NumAndar)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Area)
                .IsRequired()
                .HasColumnType("numeric");

            builder.Property(p => p.Fracao)
                .IsRequired()
                .HasColumnType("numeric");

            builder.Property(p => p.AndarId)
                .HasDefaultValueSql("uuid")
                .IsRequired();

            builder.Property(p => p.Proprietario)
                .HasColumnType("uuid");

            builder.ToTable("Imovel");
        }
    }
}