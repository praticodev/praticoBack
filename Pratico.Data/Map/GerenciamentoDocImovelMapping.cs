using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class GerenciamentoDocImovelMapping : IEntityTypeConfiguration<GerenciamentoDocImovel>
    {
        public void Configure(EntityTypeBuilder<GerenciamentoDocImovel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DocEnviado)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.DocValidado)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.EmailEnviado)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.EmailLido)
                .IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.ImovelId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            // 1 : 1 => Imovel : DocImovel
            builder.HasOne(f => f.Imovel)
                .WithOne(e => e.DocImovel);

            builder.ToTable("GerenciamentoDocImovel");
        }
    }
}
