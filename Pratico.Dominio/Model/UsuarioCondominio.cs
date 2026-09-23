using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class UsuarioCondominio
    {
        [Key]
        public Guid UsuarioId { get; set; }
        public Guid CondominioId { get; set; }
        public virtual Condominio Condominio { get; set; }
    }
}
