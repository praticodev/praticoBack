using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class RemessaInterna : Entity
    {
        public string Numero { get; set; }
        public Guid RemessaExternaId { get; set; }        
        public Guid CondominioId { get; set; }
        public Guid ConjuntoId { get; set; }
        public Guid AndarId { get; set; }
        public Guid ImovelId { get; set; }
        public Guid PessoaId { get; set; }
        public int QuantidadeItens { get; set; }
        public string? CodigoRetirada { get; set; }
        public TipoFormatoPacoteRemessa TipoPacote { get; set; }
        public Guid? OperadorId { get; set; }
        public string Imagem { get; set; }
        public DateTime? DataEntrega { get; set; }
        public Guid? OperadorRetiradaId { get; set; }
        public Guid? PessoaRetiradaId { get; set; }
        [NotMapped]
        public string NomeMoradorRetirada { get; set; }
        public virtual RemessaExterna RemessaExterna { get; set; }
        public virtual Condominio Condominio { get; set; }
        public virtual Conjunto Conjunto { get; set; }
        public virtual Andar Andar { get; set; }
        public virtual Imovel Imovel { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Operador Operador { get; set; }
    }
}
