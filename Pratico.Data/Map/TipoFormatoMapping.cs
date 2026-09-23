using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class TipoFormatoMapping : IEntityTypeConfiguration<TipoFormato>
    {
        public void Configure(EntityTypeBuilder<TipoFormato> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Codigo)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("TipoFormato");
        }
    }
}
