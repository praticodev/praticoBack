using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class AndarMapping : IEntityTypeConfiguration<Andar>
    {
        public void Configure(EntityTypeBuilder<Andar> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Observacoes)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NumTorre)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.NumAndarInterno)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.ConjuntoId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            // 1 : N => Condominio : Conjuntos
            builder.HasMany(f => f.Imoveis)
                .WithOne(p => p.Andar)
                .HasForeignKey(p => p.AndarId);

            builder.ToTable("Andar");

        }
    }
}