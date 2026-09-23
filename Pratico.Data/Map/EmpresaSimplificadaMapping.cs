using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class EmpresaSimplificadaMapping : IEntityTypeConfiguration<EmpresaSimplificada>
    {
        public void Configure(EntityTypeBuilder<EmpresaSimplificada> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Cnpj)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(p => p.NomeFantasia)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.RazaoSocial)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Tipo)
                .HasDefaultValueSql("smallint")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Operador)
                .HasColumnType("uuid");

            // 1 : N => Condominio : Conjuntos
            builder.HasMany(f => f.Remessas)
                .WithOne(p => p.EmpresaSimplificada)
                .HasForeignKey(p => p.EmpresaSimplificadaId);

            builder.ToTable("EmpresaSimplificada");
        }
    }
}
