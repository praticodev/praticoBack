using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Enums;
using System;

namespace Pratico.Api.ViewModels
{
    public class MoradorViewModel
    {

        public Guid Id { get; set; }
        public Guid ImovelId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime DataNascimento { get; set; }
        public TipoEstadoCivil EstadoCivil { get; set; }
        public TipoSexo Sexo { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public bool Ativo { get; set; }
        public string Ddd { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int CodCondominio { get; set; }
        public string Imagem { get; set; }
    }

    public class MoradorAppViewModel
    {
        public Guid ImovelId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime DataNascimento { get; set; }
        public TipoEstadoCivil EstadoCivil { get; set; }
        public TipoSexo Sexo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int CodCondominio { get; set; }        
    }

    public class MoradorCadastroViewModel
    {
        public Guid Id { get; set; }
        public Guid ImovelId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime DataNascimento { get; set; }
        public TipoEstadoCivil EstadoCivil { get; set; }
        public TipoSexo Sexo { get; set; }
        //public TipoPessoa TipoPessoa { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        //public bool Ativo { get; set; }
        //public string Ddd { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        //public bool Proprietario { get; set; }
        //public bool Inquilino { get; set; }
        public int CodCondominio { get; set; }
    }
}
