using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ImoveisProprietario
    {
        public ImoveisProprietario()
        {
            Imoveis = new List<Imovel>();
        }
        public string NomeCondominio { get; set; }
        public string NomeConjunto{ get; set; }
        public List<Imovel> Imoveis { get; set; }
    }
}
