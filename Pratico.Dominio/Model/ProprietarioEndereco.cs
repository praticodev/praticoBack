using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class ProprietarioEndereco
    {
        //public Proprietario Proprietario { get; set; }
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
        //public Endereco Endereco { get; set; }
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
