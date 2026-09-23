using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class FilaEspera : Entity
    {
        public Guid PessoaId { get; set; }
        public Guid AgendaAreaLazerId { get; set; }
    }
}
