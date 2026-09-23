using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class VendedorRemessa : Entity
    {
        //public Guid RemessaExternaId { get; set; }
        //public Guid EmpresaSimplificadaId { get; set; }
        public virtual RemessaExterna RemessaExterna { get; set; }
        public virtual EmpresaSimplificada EmpresaSimplificada { get; set; }

    }
}
