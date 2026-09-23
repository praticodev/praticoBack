using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class RemessaInternaMapping : IEntityTypeConfiguration<RemessaInterna>
    {
        public void Configure(EntityTypeBuilder<RemessaInterna> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Numero)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.CodigoRetirada)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(p => p.Imagem)
                .HasColumnType("varchar(200)");

            //builder.Property(p => p.RemessaExternaId)
            //    .IsRequired()
            //    .HasColumnType("uuid");

            builder.Property(p => p.CondominioId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.ConjuntoId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.AndarId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.ImovelId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.PessoaId)
                .IsRequired()
                .HasColumnType("uuid");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.OperadorId)
                .HasColumnType("uuid");

            builder.Property(p => p.QuantidadeItens)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.TipoPacote)
                .HasColumnType("smallint");

            builder.Property(p => p.DataEntrega)
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.PessoaRetiradaId)
                .HasColumnType("uuid");

            builder.Property(p => p.OperadorRetiradaId)
                .HasColumnType("uuid");

            builder.ToTable("RemessaInterna"); 
        }
    }
}