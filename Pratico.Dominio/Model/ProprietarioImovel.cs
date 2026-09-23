using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ProprietarioImovel
    {
        public Guid PessoaId { get; set; }
        public Guid ImovelId { get; set; }
        public bool Validado { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Imovel Imovel { get; set; }
    }
}
