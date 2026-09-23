using System;

namespace Pratico.Dominio.Model
{
    public class SyncLog
    {
        public long Token { get; set; }
        public Guid CondominioId { get; set; }
        public Guid ImovelId { get; set; }
        public string Entidade { get; set; }
        public Guid RegistroId { get; set; }
        public string Operacao { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
