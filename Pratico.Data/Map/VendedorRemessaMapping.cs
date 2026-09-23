using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class VendedorRemessaMapping : IEntityTypeConfiguration<VendedorRemessa>
    {
        public void Configure(EntityTypeBuilder<VendedorRemessa> builder)
        {
            builder.HasKey(p => p.Id);

            //builder.Property(p => p.RemessaExternaId)
            //    .HasDefaultValueSql("'uuid'")
            //    .IsRequired();

            //builder.Property(p => p.EmpresaSimplificadaId)
            //    .HasDefaultValueSql("'uuid'")
            //    .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("VendedorRemessa");

        }
    }
}
