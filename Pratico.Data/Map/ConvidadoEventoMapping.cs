using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Pratico.Dominio.Enums;

namespace Pratico.Data.Map
{
    public class ConvidadoEventoMapping : IEntityTypeConfiguration<ConvidadoEvento>
    {
        public void Configure(EntityTypeBuilder<ConvidadoEvento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.MoradorId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.ListaEventoId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.DataChegada)
                .HasDefaultValueSql("Timestamp(0)")
                .IsRequired();

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NomeAnfitriao)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NumDoc)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.TipoDoc)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("ConvidadoEvento");
        }
    }
}