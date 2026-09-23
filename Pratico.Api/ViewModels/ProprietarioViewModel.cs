using Microsoft.AspNetCore.Http;
using System;

namespace Pratico.Api.ViewModels
{
    public class DocsPropsViewModel
    {
        public Guid IdProprietario { get; set; }
        public IFormFile DocImovel { get; set; }
        public IFormFile DocProp { get; set; }
        public IFormFile DocEscritura { get; set; }
        public IFormFile DocCpf { get; set; }
        public IFormFile DocRg { get; set; }
        public IFormFile DocCertCasamento { get; set; }
        public IFormFile DocCompRes { get; set; }
    }

    public class DadosPropsViewModel
    {
        public DadosPropsViewModel()
        {
            
            if (Cnpj != null && Cnpj.Length > 0) 
                RazaoSocial = Nome;
        }
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int TipoDocumento { get; set; }
        public string NumDoc { get; set; }
        public string Cpf { get; set; }
        public int Sexo { get; set; }
        public int EstadoCivil { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public string DataNascimento { get; set; }
        public int Cargo { get; set; }
        public string IniPeriodo { get; set; }
        public string FimPeriodo { get; set; }
        public bool AssinaDoc { get; set; }
        public Guid ImovelId { get; set; }
        public Guid Entidade { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Referencia { get; set; }
        public string Cep { get; set; }
        public int UF { get; set; }
        public bool Cobranca { get; set; }
        public string Cidade { get; set; }
    }
}
