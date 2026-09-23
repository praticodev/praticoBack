using System;

namespace Pratico.Dominio.Model
{
    public class Contato : Entity
    {
        public Guid Entidade { get; set; }
        public int Ddd { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Principal { get; set; } 
        public Guid CondominioId { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
