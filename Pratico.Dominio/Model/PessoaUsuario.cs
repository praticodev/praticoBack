using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class PessoaUsuario
    {
        [Key]
        public Guid UsuarioId { get; set; }
        public Guid PessoaId { get; set; }
        public Guid CondominioId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Condominio Condominio { get; set; }
    }
}
