using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class MoradorImovel
    {
        public Guid PessoaId { get; set; }
        public Guid ImovelId { get; set; }
        public Guid CondominioId { get; set; }
        public virtual Pessoa Pessoa { get;set; }
        public virtual Imovel Imovel { get; set; }
        public virtual Condominio Condominio { get; set; }
    }
}
