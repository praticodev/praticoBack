using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class GerenciamentoDocImovel : Entity
    {
        public Guid ImovelId { get; set; }
        public bool DocEnviado { get; set; }
        public bool EmailEnviado { get; set; }
        public bool EmailLido { get; set; }
        public bool DocValidado { get; set; }
        public Imovel Imovel { get; set; }
    }
}
