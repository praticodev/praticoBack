using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class ContatoViewModel
    {
        //[Key]
        public Guid Id { get; set; }
        public int Ddd { get; set; }
        public Guid Entidade { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Principal { get; set; }
    }
}
