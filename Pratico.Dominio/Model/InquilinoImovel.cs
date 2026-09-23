using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class InquilinoImovel
    {
        public Guid PessoaId { get; set; }
        public Guid ImovelId { get; set; }
        public bool Ativo { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Imovel Imovel { get; set; }
    }
}
