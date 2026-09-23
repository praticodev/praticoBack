using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class ImovelViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AndarId { get; set; }
        public int Conjunto { get; set; }
        public int NumAndar { get; set; }
        public string NumImovel { get; set; }
        public string Area { get; set; }
        public int Numimovelinterno { get; set; }
        public string Fracao { get; set; }
        public DocImovelViewModel DocImovel { get; set; }
    }

    public class ImovelCondominioViewModel
    {
        public string NomeCondominio { get; set; }
        public string NomeProprietario { get; set; }
        public string Conjunto { get; set; }
        public int Andar { get; set; }
        public ImovelViewModel Imovel { get; set; }
        public List<string> Moradores { get; set; }
    }

    public class ImoveisProprietarioViewModel
    {
        public string NomeCondominio { get; set; }
        public string NomeConjunto { get; set; }
        public List<ImovelViewModel> Imoveis { get; set; }
    }

    public class ImoveilPropriedadeViewModel
    {
        public Guid ImovelId { get; set; }
        public Guid ProprietarioId { get; set; }
    }

    public class DocsImovelViewModel
    {
        public IFormFile Doc { get; set; }
        public Guid IdImovel { get; set; }
        public TipoDocumento TipoDoc { get; set; }
    }

    public class ImoveDocViewModel
    {
        public Guid Id { get; set; }
        public string NomeDoc { get; set; }
        public TipoDocumento TipoDoc { get; set; }
        public Guid ImovelId { get; set; }
    }
}
