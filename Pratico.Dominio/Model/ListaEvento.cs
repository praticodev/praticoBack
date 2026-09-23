using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ListaEvento : Entity
    {
        public Guid AgendaAreaLazerId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string NomeArea { get; set; }
        public string NomeDonoEvento{ get; set; }
        public virtual AgendaAreaLazer AgendaAreaLazer { get; set; }
        public IEnumerable<ConvidadoEvento> Convidaddos { get; set; }

    }
}
