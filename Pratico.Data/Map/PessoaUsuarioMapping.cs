using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class PessoaUsuarioMapping
    {
        public void Configure(EntityTypeBuilder<PessoaUsuario> builder)
        {
            builder.HasKey(p => new { p.UsuarioId, p.PessoaId });

            builder.Property(p => p.UsuarioId)
                .HasColumnName("UsuarioId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.PessoaId)
                .HasColumnName("PessoaId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasColumnName("CondominioId")
                .HasDefaultValueSql("'uuid'");

            builder.HasOne(p => p.Pessoa)
                .WithMany(p => p.PessoaUsuario)
                .HasForeignKey(p => p.PessoaId);

            builder.ToTable("PessoaUsuario");
        }
    }
}
