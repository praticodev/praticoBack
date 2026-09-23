using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class OperadorMapping : IEntityTypeConfiguration<Operador>
    {
        public void Configure(EntityTypeBuilder<Operador> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Rg)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Cpf)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.DataNascimento)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Ativo)
                .HasColumnType("boolean");

            builder.Property(p => p.Sexo)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.ToTable("Operador");
        }
    }
}
