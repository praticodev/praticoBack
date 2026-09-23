using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class UsuarioSistema
    {
        [Key]
        public Guid UsuarioId { get; set; }
        public Guid CondominioId { get; set; }
        public string Nome { get; set; }
        public int Perfil { get; set; }
        public virtual Condominio Condominio { get; set; }
    }
}
