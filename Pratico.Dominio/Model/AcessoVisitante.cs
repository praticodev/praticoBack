using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class AcessoVisitante : Entity
    {
        public Guid CondominioId { get; set; }
        public Guid ConjuntoId { get; set; }
        public Guid AndarId { get; set; }
        public Guid ImovelId { get; set; }
        public Guid PessoaId { get; set; }
        public string NomeVisitante { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumDoc { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}
