using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Model
{
    public class PrestadorServico : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Celular { get; set; }
        public string Imagem { get; set; }
        public Guid TipoPrestadorServicoId { get; set; }
        public Guid CondominioId { get; set; }
        public Guid ImovelId { get; set; }
    }
}
