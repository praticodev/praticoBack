using System;
using System.ComponentModel.DataAnnotations;

namespace Pratico.Api.ViewModels
{
    public class PrestadorServicoViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        public string Rg { get; set; }

        [Required]
        public string Celular { get; set; }

        public string Imagem { get; set; }

        [Required]
        public Guid TipoPrestadorServicoId { get; set; }

        [Required]
        public Guid ImovelId { get; set; }

        [Required]
        public int CodCondominio { get; set; }
    }
}
