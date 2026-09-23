using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class CondominioViewModel
    {
        public Guid Id { get; set; }
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public int Finalidade { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public string RazaoSocial { get; set; }
        public int TipoCondominio { get; set; }
        public bool Ativo { get; set; }
        public decimal Fracao { get; set; }
        public decimal Area { get; set; }
        public int DiaVencimento { get; set; }
        public int CodCondominio { get; set; }
        //public Guid UsuarioId { get; set; }
    }

    public class ContatoCondominioViewModel
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid Entidade { get; set; }
        public int Ddd { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Principal { get; set; }
    }

    public class CondominioCompletoViewModel
    {
        public Guid Id { get; set; }
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public string NomeProprietario { get; set; }
        public int Finalidade { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public string RazaoSocial { get; set; }
        public int TipoCondominio { get; set; }
        public bool Ativo { get; set; }
        public decimal Fracao { get; set; }
        public decimal Area { get; set; }
        public int DiaVencimento { get; set; }
        public Guid UsuarioId { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public ContatoCondominioViewModel Contato { get; set; }
        public RepresentanteLegalViewModel RepresentanteLegal { get; set; }
        public IEnumerable<ConjuntoViewModel> Conjuntos { get; set; }
    }
}
