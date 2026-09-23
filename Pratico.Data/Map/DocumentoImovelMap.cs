using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class DocumentoImovelMap : IEntityTypeConfiguration<DocumentoImovel>
    {
        public void Configure(EntityTypeBuilder<DocumentoImovel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.NomeDoc)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.ImovelId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Validado)
                .HasColumnType("boolean");
            
            builder.Property(p => p.TipoDoc)
                .HasDefaultValueSql("integer")
                .IsRequired();

            builder.Property(p => p.Url)
                .HasDefaultValueSql("varchar(200)")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("DocumentoImovel");
        }
    }
}
