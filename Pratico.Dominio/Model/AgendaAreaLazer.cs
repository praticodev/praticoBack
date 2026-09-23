using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class AgendaAreaLazer : Entity
    {
        public Guid PessoaId { get; set; }
        public Guid AreaLazerId { get; set; }
        public Guid CondominioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Autorizado { get; set; } = 0;
        public virtual Pessoa Pessoa { get; set; }
        public virtual AreaLazer AreaLazer { get; set; }
        public virtual Condominio Condominio { get; set; }
    }
}
