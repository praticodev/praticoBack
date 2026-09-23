using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class RemessaExterna : Entity
    {
        public string Numero { get; set; } = string.Empty;   
        public int QuantidadeItens { get; set; } = 0;
        public int QuantidadeLancados { get; set; } = 0;
        public Guid CondominioId { get; set; }
        public Guid EmpresaSimplificadaId { get; set; }
        public bool Interna { get; set; }
        public string Imagem { get; set; } = string.Empty;
        public Guid? OperadorId { get; set; }
        [NotMapped]
        public Departamento Departamento { get; set; }
        public virtual Condominio Condominio { get; set; }
        public virtual Operador Operador { get; set; }
        public virtual EmpresaSimplificada EmpresaSimplificada { get; set; }
        public IEnumerable<RemessaInterna> RemessasInternas { get; set; }
    }
}
