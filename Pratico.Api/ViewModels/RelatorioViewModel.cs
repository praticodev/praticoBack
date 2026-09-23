using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;

namespace Pratico.Api.ViewModels
{
    public class RelatorioRemessaExternaViewModel
    {
        //[FromHeader]
        public int Codigo { get; set; }
        //[FromHeader]
        public DateTime? Inicio { get; set; }
        //[FromHeader]
        public DateTime? Fim { get; set; }
        //[FromHeader]
        public Guid? Operador { get; set; }
    }
}
