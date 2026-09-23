using Pratico.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pratico.Dominio.Model
{
    public class Pessoa : Entity
    {
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Cnpj { get; set; }
        public string Cnh { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public TipoEstadoCivil EstadoCivil { get; set; }
        public TipoSexo Sexo { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public bool UsarEndImovel { get; set; }
        public Guid CondominioId { get; set; }
        public string Imagem { get; set; }
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string Ddd { get; set; }
        [NotMapped]
        public string Telefone { get; set; }
        public virtual IEnumerable<MoradorImovel> MoradoresImovel { get; set; }
        public virtual IEnumerable<InquilinoImovel> InquilinosImovel { get; set; }
        public virtual IEnumerable<ProprietarioImovel> ProprietariosImovel { get; set; }
        public virtual IEnumerable<PessoaUsuario> PessoaUsuario { get; set; }
        public virtual IEnumerable<AcessoVisitante> AcessoVisitantes { get; set; }
    }
}
