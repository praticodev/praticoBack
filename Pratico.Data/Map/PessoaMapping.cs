using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Cpf)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.DataNascimento)
                //.IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.EstadoCivil)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Cnpj)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Cnh)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.InscEstadual)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.InscMunicipal)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.NomeFantasia)
                //.IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Sexo)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.EstadoCivil)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.TipoPessoa)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.TipoDocumento)
                .HasColumnType("smallint");

            builder.Property(p => p.UsarEndImovel)
                //.IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'");

            builder.Property(p => p.Imagem)
                .HasDefaultValueSql("varchar(200)");

            builder.ToTable("Pessoa");
        }
    }
}
