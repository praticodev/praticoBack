using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Remetente
    {
        EmpresaSimplificada EmpresaSimplificada { get; set; }
        public string DocumentoFiscal { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
    }
}
