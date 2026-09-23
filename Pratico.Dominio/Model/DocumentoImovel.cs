using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class DocumentoImovel : Entity
    {
        public string NomeDoc { get; set; }
        public TipoDocumento TipoDoc { get; set; }
        public bool Validado { get; set; }
        public Guid ImovelId { get; set; }
        public string Url { get; set; }
    }
}
