using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pratico.Api.ViewModels
{
    public class VeiculoFabricanteViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
    }

    public class VeiculoCorViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
    }

    public class VeiculoTipoViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
    }

    public class VeiculoViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ImovelId { get; set; }

        [Required]
        public Guid VeiculoFabricanteId { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public int Tipo { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public int CodCondominio { get; set; }
    }

    public class VeiculoAppViewModel
    {

        [Required]
        public Guid ImovelId { get; set; }

        [Required]
        public Guid VeiculoFabricanteId { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public int Tipo { get; set; }
        public int CodCondominio { get; set; }

    }
}
