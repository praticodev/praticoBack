using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class UsuarioCondominioMapping
    {
        public void Configure(EntityTypeBuilder<UsuarioCondominio> builder)
        {
            builder.HasKey(p => new { p.UsuarioId, p.CondominioId });

            builder.Property(p => p.UsuarioId)
                .HasColumnName("UsuarioId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasColumnName("CondominioId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.ToTable("UsuarioCondominio");
        }
    }
}
