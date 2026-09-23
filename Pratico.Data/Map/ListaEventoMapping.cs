using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class ListaEventoMapping : IEntityTypeConfiguration<ListaEvento>
    {
        public void Configure(EntityTypeBuilder<ListaEvento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AgendaAreaLazerId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataFim)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataInicio)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.NomeArea)
                .HasDefaultValueSql("varchar(50)")
                .IsRequired();

            builder.Property(p => p.NomeDonoEvento)
                .HasDefaultValueSql("varchar(200)");

            // 1 : N => Condominio : Conjuntos
            builder.HasMany(f => f.Convidaddos)
                .WithOne(p => p.ListaEvento)
                .HasForeignKey(p => p.ListaEventoId);

            builder.ToTable("ListaEvento");
        }
    }
}
