using System;

namespace Pratico.Api.ViewModels
{
    public class DocImovelViewModel
    {
        public Guid Id { get; set; }
        public Guid ImovelId { get; set; }
        public bool DocEnviado { get; set; }
        public bool EmailEnviado { get; set; }
        public bool EmailLido { get; set; }
        public bool DocValidado { get; set; }
        public ImovelViewModel Imovel { get; set; }
    }
}
