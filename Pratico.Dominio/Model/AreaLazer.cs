using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class AreaLazer : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Guid CondominioId { get; set; }
        public int Capacidade { get; set; }
        public IEnumerable<AgendaAreaLazer> Agenda { get; set; }
    }
}
