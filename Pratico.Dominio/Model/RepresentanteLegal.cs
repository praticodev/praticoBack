using System;
using System.Collections.Generic;

namespace Pratico.Dominio.Model
{
    public class RepresentanteLegal : Entity
    {
        public string NomeRep { get; set; }
        public int TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public string Cpf { get; set; }
        public int Sexo { get; set; }
        public int EstCivil { get; set; }
        public string RazaoSocialRep { get; set; }
        public string NomeFantasiaRep { get; set; }
        public string CnpjRep { get; set; }
        public string InscEstadualRep { get; set; }
        public string InscMunicipalRep { get; set; }
        public DateTime DataNascimentoRep { get; set; }
        public int Cargo { get; set; }
        public DateTime IniPeriodo { get; set; }
        public DateTime FimPeriodo { get; set; }
        public bool AssinaDoc { get; set; }
        public Guid CondominioId { get; set; }
        public virtual IEnumerable<Condominio> Condominios { get; set; }
    }
}