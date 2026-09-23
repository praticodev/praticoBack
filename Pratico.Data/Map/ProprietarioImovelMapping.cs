using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class ProprietarioImovelMapping : IEntityTypeConfiguration<ProprietarioImovel>
    {
        public void Configure(EntityTypeBuilder<ProprietarioImovel> builder)
        {
            builder.HasKey(p => new { p.ImovelId, p.PessoaId });

            builder.Property(p => p.PessoaId)
                .HasColumnName("PessoaId")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(p => p.ImovelId)
                .HasColumnName("ImovelId")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(p => p.Validado)
                .HasColumnName("Validado")
                .HasColumnType("boolean");

            builder.HasOne(p => p.Pessoa)
                .WithMany(p => p.ProprietariosImovel)
                .HasForeignKey(p => p.PessoaId);

            builder.HasOne(p => p.Imovel)
                .WithMany(p => p.ProprietariosImovel)
                .HasForeignKey(p => p.ImovelId);

            builder.ToTable("ProprietarioImovel");
        }
    }
}
