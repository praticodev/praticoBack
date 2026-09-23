using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Endereco : Entity
    {
        public Guid Entidade { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Referencia { get; set; }
        public string Cep { get; set; }
        public int UF { get; set; }
        public bool Cobranca { get; set; }
        public string Cidade { get; set; }
    }
}
