using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ConvidadoEvento : Entity
    {
        public Guid ListaEventoId { get; set; }
        public string Nome { get; set; }
        public TipoDocumento TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public Guid MoradorId { get; set; }
        public string NomeAnfitriao { get; set; }
        public DateTime DataChegada { get; set; }
        public ListaEvento ListaEvento { get; set; }
    }
}
