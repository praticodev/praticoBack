using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class AndarViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Observacoes { get; set; }
        public int NumTorre { get; set; }
        public int NumandarInterno { get; set; }
        public Guid Conjuntoid { get; set; }
        public IEnumerable<ImovelViewModel> Imoveis { get; set; }
    }
}
