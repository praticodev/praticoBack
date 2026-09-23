using System;

namespace Pratico.Dominio.Model
{
    public class VisitanteImovel : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Celular { get; set; }
        public string Imagem { get; set; }
        public Guid CondominioId { get; set; }
        public Guid ImovelId { get; set; }
    }
}
