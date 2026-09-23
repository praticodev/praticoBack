using System;
using System.ComponentModel.DataAnnotations;

namespace Pratico.Api.ViewModels
{
    public class EmailDocProprietarioViewModel
    {
        public Guid IdImovel { get; set; }
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }
    }

    public class EmailCadastroProprietarioViewModel
    {
        public Guid IdImovel { get; set; }
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }
    }

    public class EmailConviteAppViewModel
    {
        [Required(ErrorMessage = "Morador obrigatório")]
        public string IdMorador { get; set; }
        public int CodCondominio { get; set; }
    }
}
