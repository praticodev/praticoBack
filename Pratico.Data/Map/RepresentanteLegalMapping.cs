using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class RepresentanteLegalMapping : IEntityTypeConfiguration<RepresentanteLegal>
    {
        public void Configure(EntityTypeBuilder<RepresentanteLegal> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.Cargo)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.Cpf)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.DataNascimentoRep)
                //.IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.EstCivil)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.CnpjRep)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.InscEstadualRep)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.InscMunicipalRep)
                //.IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.NomeFantasiaRep)
                //.IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.RazaoSocialRep)
                //.IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.FimPeriodo)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.IniPeriodo)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            builder.Property(p => p.NomeRep)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.NumDoc)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(p => p.Sexo)
                //.IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.TipoDoc)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.AssinaDoc)
                //.IsRequired()
                .HasColumnType("boolean");

            builder.Property(p => p.CondominioId)
                .HasDefaultValueSql("'uuid'")
                .IsRequired();

            builder.ToTable("RepresentanteLegal");
        }
    }
}
