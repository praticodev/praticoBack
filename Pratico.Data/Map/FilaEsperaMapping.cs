using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class FilaEsperaMapping : IEntityTypeConfiguration<FilaEspera>
    {
        public void Configure(EntityTypeBuilder<FilaEspera> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PessoaId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.AgendaAreaLazerId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.ToTable("FilaEspera");
        }
    }
}
