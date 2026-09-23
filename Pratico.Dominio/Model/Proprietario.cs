using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Proprietario : Pessoa
    {
        public List<Imovel> Imoveis { get; set; }
    }
}
