using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class EdificacaoCondominio : Entity
    {
        public Guid CondominioId { get; set; }
        public string Nome { get; set; }
        public Condominio Condominio { get; set; }
    }
}
