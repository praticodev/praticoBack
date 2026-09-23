using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Map
{
    public class MoradorImovelMapping : IEntityTypeConfiguration<MoradorImovel>
    {
        public void Configure(EntityTypeBuilder<MoradorImovel> builder)
        {
            builder.HasKey(p => new { p.ImovelId, p.PessoaId });

            builder.Property(p => p.PessoaId)
                .HasColumnName("PessoaId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.ImovelId)
                .HasColumnName("ImovelId")
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.Property(p => p.CondominioId)
                .HasColumnName("CondominioId")
                .HasDefaultValueSql("'uuid'");

            builder.HasOne(p => p.Pessoa)
                .WithMany(p => p.MoradoresImovel)
                .HasForeignKey(p => p.PessoaId);

            builder.HasOne(p => p.Imovel)
                .WithMany(p => p.MoradoresImovel)
                .HasForeignKey(p => p.ImovelId);

            builder.ToTable("MoradorImovel");
        }
    }
}
