using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Remessa : Entity
    {
        public string Numero { get; set; }
        public DateTime DataEntrada { get; set; }
        public Guid EmpresaSimplificadaId { get; set; }
        public Guid CondominioId { get; set; }
        public Guid ConjuntoId { get; set; }
        public int QuantidadeItens { get; set; }
        public virtual EmpresaSimplificada EmpresaSimplificada { get; set; }
        public virtual Condominio Condominio { get; set; }
        public virtual Conjunto Conjunto { get; set; }
    }
}
