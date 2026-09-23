using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Departamento : Entity
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public Guid CondominioId { get; set; }
        public virtual Condominio Condominio { get; set; } 
    }
}
