using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pratico.Dominio.Model;

namespace Pratico.Data.Map
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TipoUsuario)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(p => p.DataCadastro)
                .IsRequired()
                .HasColumnType("Timestamp(0)");

            //1 : N => Usuario : Condominios
            //builder.HasMany(f => f.Condominios)
            //    .WithOne(p => p.Usuario)
            //    .HasForeignKey(p => p.UsuarioId);
        }
    }
}
