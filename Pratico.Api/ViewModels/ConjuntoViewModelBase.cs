using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class ConjuntoViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int NumTorre { get; set; }
        public Guid CondominioId { get; set; }
        public int UsuarioId { get; set; }
        public IEnumerable<AndarViewModel> Andares { get; set; }
    }

    public class ConjuntoIguaisViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public int NumTorre { get; set; }
        public string NomeTorre { get; set; }
        public string Descricao { get; set; }
        public decimal Fracao { get; set; }
        public decimal Area { get; set; }
        public int QtdAndares { get; set; }
        public int QtdImoveis { get; set; }
        public string NumPrimeiroImovel { get; set; }
        public Guid CondominioId { get; set; }
    }
}
