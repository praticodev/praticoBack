using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class EmpresaSimplificada : Entity
    {
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public int Tipo { get; set; }
        public Guid CondominioId { get; set; }
        [NotMapped]
        public int Codigo { get; set; }
        public Guid? Operador { get; set; }
        public Condominio Condominio { get; set; }
        public virtual IEnumerable<RemessaExterna> Remessas { get; set; }

        //public Endereco Endereco { get; set; }
    }
}
