using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class AcessoVisitanteMapping : IEntityTypeConfiguration<AcessoVisitante>
    {
        public void Configure(EntityTypeBuilder<AcessoVisitante> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AndarId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.ConjuntoId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.ImovelId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.PessoaId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.TipoDocumento)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.NomeVisitante)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NumDoc)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("AcessoVisitante");
        }
    }
}
