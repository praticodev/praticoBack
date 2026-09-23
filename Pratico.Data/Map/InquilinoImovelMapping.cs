using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class InquilinoImovelMapping : IEntityTypeConfiguration<InquilinoImovel>
    {
        public void Configure(EntityTypeBuilder<InquilinoImovel> builder)
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

            builder.Property(p => p.Ativo)                
                .HasColumnType("boolean")
                .IsRequired();

            builder.HasOne(p => p.Pessoa)
                .WithMany(p => p.InquilinosImovel)
                .HasForeignKey(p => p.PessoaId);

            builder.HasOne(p => p.Imovel)
                .WithMany(p => p.InquilinosImovel)
                .HasForeignKey(p => p.ImovelId);

            builder.ToTable("InquilinoImovel");
        }
    }
}
