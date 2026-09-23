using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pratico.Dominio.Model
{
    public class Condominio : Entity
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
        public int CodCondominio { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public IEnumerable<Conjunto> Conjuntos { get; set; }
        public IEnumerable<RemessaExterna> Remessas { get; set; }
        public IEnumerable<UsuarioCondominio> UsuarioCondominio { get; set; }
        public IEnumerable<UsuarioSistema> UsuarioSistema { get; set; }
        public IEnumerable<EmpresaSimplificada> EmpresaSimplificada { get; set; }
        public IEnumerable<AreaLazer> AreasLazer { get; set; }
        public IEnumerable<TipoAreaLazer> TiposAreaLazer { get; set; }
        public IEnumerable<AgendaAreaLazer> AgendaAreaLazer { get; set; }
        public IEnumerable<Operador> Operador { get; set; }
        public IEnumerable<PessoaUsuario> PessoaUsuario { get; set; }
    }
}
