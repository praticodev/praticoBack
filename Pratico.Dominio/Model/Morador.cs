using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Morador : Pessoa
    {
        public Guid ImovelId { get;set; }   
        public bool Ativo { get; set; }
    }
}
