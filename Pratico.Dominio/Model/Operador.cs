using Comtele.Sdk.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class Operador : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Sexo { get; set; }
        public bool Ativo { get; set; }
        public Guid CondominioId { get; set; }
        public virtual Condominio Condominio { get; set; }
        public virtual IEnumerable<RemessaExterna> RemessaExterna { get; set; }
        public virtual IEnumerable<RemessaInterna> RemessaInterna { get; set; }
    }
}
