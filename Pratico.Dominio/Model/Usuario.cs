using System.Collections.Generic;

namespace Pratico.Dominio.Model
{
    public class Usuario : Entity
    {
        public int TipoUsuario { get; set; }
        public string Nome { get; set; }
        //public virtual IEnumerable<Condominio> Condominios { get; set; }
    }
}
