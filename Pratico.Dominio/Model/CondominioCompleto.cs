using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class CondominioCompleto : Entity
    {
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public int Finalidade { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public string RazaoSocial { get; set; }
        public int TipoCondominio { get; set; }
        public decimal Fracao { get; set; }
        public decimal Area { get; set; }
        public bool Ativo { get; set; }
        public int DiaVencimento { get; set; }
        public Guid UsuarioId { get; set; }
        public Endereco Endereco { get; set; }
        public Contato Contato { get; set; }
        public RepresentanteLegal RepresentanteLegal { get; set; }
        public IEnumerable<Conjunto> Conjuntos { get; set; }

    }
}
