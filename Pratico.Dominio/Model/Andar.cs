using System;
using System.Collections.Generic;

namespace Pratico.Dominio.Model
{
    public class Andar : Entity
    {
        public string Observacoes { get; set; }
        public int NumTorre { get; set; }
        public int NumAndarInterno { get; set; }
        public Guid ConjuntoId { get; set; }
        public Conjunto Conjunto { get; set; }
        public IEnumerable<Imovel> Imoveis { get; set; }
    }
}
