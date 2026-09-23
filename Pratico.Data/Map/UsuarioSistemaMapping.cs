using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Pratico.Data.Map
{
    public class UsuarioSistemaMapping
    {
        public void Configure(EntityTypeBuilder<UsuarioSistema> builder)
        {
            builder.HasKey(p => new { p.UsuarioId });

            builder.Property(p => p.UsuarioId)
                .HasColumnName("UsuarioId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasColumnName("CondominioId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.Nome)
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Perfil)
                .HasColumnType("int");

            builder.ToTable("UsuarioSistema");
        }
    }
}
