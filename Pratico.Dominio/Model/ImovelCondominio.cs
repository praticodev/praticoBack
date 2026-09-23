using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ImovelCondominio : Entity
    {
        public ImovelCondominio()
        {
            Moradores = new List<string>();
        }
        public string NomeCondominio { get; set; }
        public string Conjunto { get; set; }
        public int Andar { get; set; }
        public string NomeProprietario { get; set; }
        public Imovel Imovel { get; set; }
        public List<string> Moradores { get; set; }
    }
}
