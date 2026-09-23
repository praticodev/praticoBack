using Microsoft.AspNetCore.Http;
using System;

namespace Pratico.Api.ViewModels
{
    public class DocumentoImovelViewModel
    {
        public string NomeDoc { get; set; }
        public int TipoDoc { get; set; }
        public Guid ImovelId { get; set; }
        public IFormFile Doc { get; set; }
    }
}
