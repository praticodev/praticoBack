using Pratico.Dominio.Enums;
using System;

namespace Pratico.Api.ViewModels
{
    public class UsuarioMoradorViewModel
    {
        public UsuarioMoradorViewModel()
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
        public DateTime DataNascimento { get; set; }
        public int Cargo { get; set; }
        public string IniPeriodo { get; set; }
        public string FimPeriodo { get; set; }
        public Guid Entidade { get; set; }
        public int CodCondominio { get; set; }
    }
}
