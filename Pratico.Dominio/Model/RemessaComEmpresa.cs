using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class RemessaComEmpresa : Entity
    {
        public string Numero { get; set; }
        public int QuantidadeItens { get; set; }
        public Guid CondominioId { get; set; }
        public Guid EmpresaSimplificadaId { get; set; }
        public string NomeEmpresa { get; set;}
    }
}
