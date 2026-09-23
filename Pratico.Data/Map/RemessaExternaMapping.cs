using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class RemessaExternaMapping : IEntityTypeConfiguration<RemessaExterna>
    {
        public void Configure(EntityTypeBuilder<RemessaExterna> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Numero)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.CondominioId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.EmpresaSimplificadaId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.QuantidadeItens)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.QuantidadeLancados)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Interna)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.OperadorId)
                .HasColumnType("uuid");

            builder.Property(p => p.Imagem)
                .HasColumnType("varchar(200)");

            builder.ToTable("RemessaExterna");
        }
    }
}
